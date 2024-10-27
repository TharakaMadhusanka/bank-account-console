using Domain.Constants;
using GicConsole.Constants;
using GicConsole.Interface;
using System.Globalization;

namespace GicConsole.Validators
{
    internal class InputValidator : IInputValidator
    {
        public InputValidator() { }
        public bool IsValidDate(string date)
        {
            return DateTime.TryParseExact(date, CommonConstant.TransactionDateInputFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
        public bool IsValidTransactionType(string transactionType)
        {
            return transactionType?.ToUpperInvariant() == GicConstants.Withdrawal
                || transactionType?.ToUpperInvariant() == GicConstants.Deposit;
        }
        public bool IsValidTransactionAmount(string transactionAmount)
        {
            if (Decimal.TryParse(transactionAmount, NumberStyles.Currency, null, out decimal validAmount))
            {
                return Math.Round(validAmount, 2) == validAmount && validAmount > 0;
            };
            return false;
        }
        public bool IsValidInterestRate(string interestRate)
        {
            Decimal.TryParse(interestRate, NumberStyles.Currency, null, out decimal validRate);
            return validRate >= 0 && validRate <= 100;
        }
        public bool IsValidTransactionInput(string[] inputTransaction)
        {
            return IsValidDate(inputTransaction[0]) && IsValidTransactionType(inputTransaction[2]) && IsValidTransactionAmount(inputTransaction[3]);
        }

        public bool IsValidInterestRuleRequest(string[] inputTransaction)
        {
            return IsValidDate(inputTransaction[0]) && IsValidInterestRate(inputTransaction[2]);
        }
    }
}
