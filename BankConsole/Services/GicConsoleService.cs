using Domain.Dao;
using Domain.Dto;
using Domain.Enum;
using Domain.Extensions;
using Domain.GicDomainInterface;
using GicConsole.Constants;
using GicConsole.Interface;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace GicConsole.Services
{
    internal class GicConsoleService : IGicConsoleService
    {
        private readonly IInputValidator _inputValidator;
        private readonly IGicTransactionDomainService _transactionDomainService;
        private readonly IInterestRuleDomainService _interestRuleDomainService;

        public GicConsoleService(IInputValidator inputValidator,
            IGicTransactionDomainService gicTransactionDomainService,
            IInterestRuleDomainService interestRuleDomainService)
        {
            _inputValidator = inputValidator;
            _transactionDomainService = gicTransactionDomainService;
            _interestRuleDomainService = interestRuleDomainService;
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

        private async Task ActionFlow(char input)
        {

            switch (char.ToUpper(input))
            {
                case 'T':
                    InputTransactions();
                    break;
                case 'I':
                    DefineInterestRulesAsync();
                    break;
                case 'P':
                    DefineInterestRulesAsync();
                    break;
                case 'Q':
                    Console.Clear();
                    break;
                default:
                    await MainMenu(GetScreenMainMessage(GicConstants.WelcomeMessage));
                    break;
            }
        }

        #region Input Transaction
        public async void InputTransactions()
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
            //using var transactionScope = new TransactionScope();
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
                //transactionScope.Complete();
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

        private async void InputTransactionPrintStatement(InputTransactionResponseDto statement)
        {
            // thread lock
            var statementBuilder = new StringBuilder();
            statementBuilder.AppendLine(GicConstants.InputTransactionStatement.Replace("<AccountNo>", statement.AccountNo));
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
        private async void DefineInterestRulesAsync()
        {
            Console.WriteLine(GicConstants.DefineInterestRule);
            var inputTransaction = Console.ReadLine();
            // YYYYMMDD Rule InterestRate
            if (inputTransaction is null) { await MainMenu(GetScreenMainMessage(GicConstants.WelcomeMessage)); return; }
            var sanitizedInputTransactionDetails = inputTransaction.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (!_inputValidator.IsValidInterestRuleRequest(sanitizedInputTransactionDetails))
            { await MainMenu(GetScreenMainMessage(GicConstants.WelcomeMessage)); return; }
            // Transaction Scope
            try
            {
                var interestRules = await _interestRuleDomainService.InterestRuleDefineRequestHandleAsync(new InterestRuleDefineRequestParamsDao
                { Date = sanitizedInputTransactionDetails[0], InterestRate = sanitizedInputTransactionDetails[2], Rule = sanitizedInputTransactionDetails[1] });
                PrintInterestRulesStatement(interestRules);
                await MainMenu(GicConstants.NextActionConsentMessage);

            }
            catch (DbUpdateException) { Console.WriteLine("Something went wrong and we cannot create the defnition."); }
        }
        private async void PrintInterestRulesStatement(IReadOnlyCollection<InterestRuleDto> statement)
        {
            var statementBuilder = new StringBuilder();
            statementBuilder.AppendLine(GicConstants.InterestRulesStatement);
            foreach (var item in statement)
            {
                statementBuilder.AppendLine($"|{item.Date}\t" +
                    $"|{item.RuleId}\t" +
                    $"|{item.Rate}\t|\n");
            };
            Console.WriteLine(statementBuilder.ToString());
            await MainMenu(GicConstants.NextActionConsentMessage);
        }
        #endregion

        private static string GetScreenMainMessage(string titleMessage)
        {
            return GicConstants.ActionsMenuAndTitleMessage.Replace("<TitleMessage>", titleMessage);
        }

    }
}
