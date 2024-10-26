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

        public GicConsoleService(IInputValidator inputValidator, IGicTransactionDomainService gicTransactionDomainService)
        {
            _inputValidator = inputValidator;
            _transactionDomainService = gicTransactionDomainService;
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
                    InputTransactions();
                    break;
                case 'P':
                    PrintStatement();
                    break;
                case 'Q':
                    Console.Clear();
                    break;
                default:
                    await MainMenu(GetScreenMainMessage(GicConstants.WelcomeMessage));
                    break;
            }
        }

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
        private static void DefineInterestRules() { }
        private static void PrintStatement() { }

        private static string GetScreenMainMessage(string titleMessage)
        {
            return GicConstants.ActionsMenuAndTitleMessage.Replace("<TitleMessage>", titleMessage);
        }

    }
}
