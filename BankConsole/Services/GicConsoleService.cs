using Domain.Dao;
using Domain.Dto;
using Domain.Enum;
using Domain.Extensions;
using Domain.GicDomainInterface;
using GicConsole.Constants;
using GicConsole.Interface;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Transactions;

namespace GicConsole.Services
{
    internal class GicConsoleService : IGicConsoleService
    {
        private readonly IInputValidator _inputValidator;
        private readonly IGicTransactionDomainService _transactionDomainService;
        private readonly IInterestRuleDomainService _interestRuleDomainService;
        private readonly IAccountStatementDomainService _accountStatementDomainService;

        public GicConsoleService(IInputValidator inputValidator,
            IGicTransactionDomainService gicTransactionDomainService,
            IInterestRuleDomainService interestRuleDomainService,
            IAccountStatementDomainService accountStatementDomainService)
        {
            _inputValidator = inputValidator;
            _transactionDomainService = gicTransactionDomainService;
            _interestRuleDomainService = interestRuleDomainService;
            _accountStatementDomainService = accountStatementDomainService;
        }

        public async Task WelcomeAndInitTransactionsAsync()
        {
            await MainMenu(GetScreenMainMessage(GicConstants.WelcomeMessage));
            Console.ReadLine();
        }
        private async Task MainMenu(string message)
        {
            Console.WriteLine(message);
            var input = Console.ReadLine();
            if (input == null || input.Length > 1) { await MainMenu(message); return; }
            await ActionFlow(input[0]);
        }

        #region Action Manager
        private async Task ActionFlow(char input)
        {

            switch (char.ToUpper(input))
            {
                case 'T':
                    await InputTransactionsAsync();
                    break;
                case 'I':
                    await DefineInterestRulesAsync();
                    break;
                case 'P':
                    await RetrieveAccountStatementWithInterestAsync();
                    break;
                case 'Q':
                    Console.WriteLine(GicConstants.QuitMessage);
                    break;
                default:
                    await MainMenu(GetScreenMainMessage(GicConstants.WelcomeMessage));
                    break;
            }
        }
        #endregion

        #region Input Transaction
        public async Task InputTransactionsAsync()
        {
            Console.WriteLine(GicConstants.InputTransactions);
            var inputTransaction = Console.ReadLine();
            // YYYYMMDD ACNO D|W Amount(>0, 2 decimals)
            if (inputTransaction is null) { await MainMenu(GetScreenMainMessage(GicConstants.WelcomeMessage)); return; }
            var sanitizedInputTransactionDetails = inputTransaction.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (!_inputValidator.IsValidTransactionInput(sanitizedInputTransactionDetails))
            { await MainMenu(GetScreenMainMessage(GicConstants.WelcomeMessage)); return; }

            // Make Transaction
            // Deduct or Deposit to Current Account
            // Update Total Balance
            // Transaction Scope has the proble of Transactions tracking which detach the entitis
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var statement = await _transactionDomainService.InputTransactionHandleAsync(new InputTransactionRequestParamsDao
                {
                    AccountNo = sanitizedInputTransactionDetails[1],
                    TransactionDate = sanitizedInputTransactionDetails[0],
                    TransactionType = sanitizedInputTransactionDetails[2].GetTransactionTypeCode(),
                    Amount = sanitizedInputTransactionDetails[3]
                });
                // Complete Transaction will commit data to DB
                transactionScope.Complete();
                // After that Print the statement
                // If not compelte it won't Print the Statement and throw error
                // When Invalid Request what is the expectation >_<
                InputTransactionPrintStatement(statement);
                await MainMenu(GetScreenMainMessage(GicConstants.NextActionConsentMessage));
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("System encountered error and your transction is reverted! Please reach out to bank officer.");
            }
        }
        private static void InputTransactionPrintStatement(InputTransactionResponseDto statement)
        {
            // thread lock
            var statementBuilder = ConstructStringBuilder(GicConstants.InputTransactionStatement.Replace("<AccountNo>", statement.AccountNo));
            if (!statement.IsTransactionSuccess || statement.TransactionRecordsList is null)
            {
                Console.WriteLine(statementBuilder.ToString());
                return;
            };
            foreach (var item in statement.TransactionRecordsList)
            {
                statementBuilder.AppendLine($"|{item.TransactionDate}\t" +
                    $"|{item.TransactionNo}\t" +
                    $"|{((TransactionTypes)Convert.ToInt16(item.TransactionType)).GetDisplayNameShort()}\t" +
                    $"|{item.Amount.ToString("#,0.00")}\t|\n");
            };
            Console.WriteLine(statementBuilder.ToString());

        }
        #endregion

        #region Interest Rules Define
        private async Task DefineInterestRulesAsync()
        {
            Console.WriteLine(GicConstants.DefineInterestRule);
            var inputTransaction = Console.ReadLine();
            // YYYYMMDD Rule InterestRate
            if (inputTransaction is null) { await MainMenu(GetScreenMainMessage(GicConstants.WelcomeMessage)); return; }
            var sanitizedInputTransactionDetails = inputTransaction.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (!_inputValidator.IsValidInterestRuleRequest(sanitizedInputTransactionDetails))
            { await MainMenu(GetScreenMainMessage(GicConstants.WelcomeMessage)); return; }
            // Transaction Scope
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var interestRules = await _interestRuleDomainService.InterestRuleDefineRequestHandleAsync(new InterestRuleDefineRequestParamsDao
                { Date = sanitizedInputTransactionDetails[0], InterestRate = sanitizedInputTransactionDetails[2], Rule = sanitizedInputTransactionDetails[1] });
                transactionScope.Complete();
                PrintInterestRulesStatement(interestRules);
                await MainMenu(GicConstants.NextActionConsentMessage);

            }
            catch (DbUpdateException) { Console.WriteLine("Something went wrong and we cannot create the defnition."); }
        }
        private static void PrintInterestRulesStatement(IReadOnlyCollection<InterestRuleDto> statement)
        {
            var statementBuilder = ConstructStringBuilder(GicConstants.InterestRulesStatement);
            foreach (var item in statement)
            {
                statementBuilder.AppendLine($"|{item.Date}\t" +
                    $"|{item.RuleId}\t" +
                    $"|{item.Rate}\t|\n");
            };
            Console.WriteLine(statementBuilder.ToString());
        }
        #endregion

        #region Account Statement
        public async Task RetrieveAccountStatementWithInterestAsync()
        {
            Console.WriteLine(GicConstants.PrintAccountStatementMessage);
            var inputTransaction = Console.ReadLine();
            // AccountNo YYYYMM
            if (inputTransaction is null) { await MainMenu(GetScreenMainMessage(GicConstants.WelcomeMessage)); return; }
            var sanitizedInputTransactionDetails = inputTransaction.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (!_inputValidator.IsValidAccountStatementRequest(sanitizedInputTransactionDetails))
            { await MainMenu(GetScreenMainMessage(GicConstants.WelcomeMessage)); return; }
            var statement = await _accountStatementDomainService.AccountStatementHandleAsync(new AccountStatementInputRequestDao
            { AccountNo = sanitizedInputTransactionDetails[0], YearMonth = sanitizedInputTransactionDetails[1] });
            PrintAccountStatementAsync(statement);
            await MainMenu(GicConstants.NextActionConsentMessage);
        }
        private static void PrintAccountStatementAsync(IReadOnlyCollection<TransactionRecordDto> statement)
        {
            var statementBuilder = ConstructStringBuilder(GicConstants.PrintAccountStatementMessage);
            foreach (var item in statement)
            {
                statementBuilder.AppendLine($"|{item.TransactionDate}\t" +
                    $"|{item.TransactionNo}\t" +
                    $"|{item.TransactionType}\t" +
                    $"|{item.Amount.ToString("#,0.00")}\t" +
                    $"|{item.EndBalance.ToString("#,0.00")}|\n");
            };
            Console.WriteLine(statementBuilder.ToString());
        }
        #endregion

        #region Console Screen
        private static string GetScreenMainMessage(string titleMessage)
        {
            return GicConstants.ActionsMenuAndTitleMessage.Replace("<TitleMessage>", titleMessage);
        }
        #endregion

        private static StringBuilder ConstructStringBuilder(string sectionMessage)
        {
            return new StringBuilder()
                 .Append(GicConstants.SectionBreaker)
                 .AppendLine(sectionMessage);
        }
    }
}
