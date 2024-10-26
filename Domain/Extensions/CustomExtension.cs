using Domain.Constants;
using Domain.Enum;

namespace Domain.Extensions
{
    public static class CustomExtensions
    {
        public static string GetDisplayNameShort(this TransactionTypes transactionType)
        {
            return transactionType == TransactionTypes.Withdraw ? DomainConstants.WithdrawalShortForm : DomainConstants.DepositShortForm;
        }

        public static TransactionTypes GetTransactionTypeCode(this string decodedTransactionType)
        {
            return decodedTransactionType.Equals(DomainConstants.WithdrawalShortForm, StringComparison.InvariantCultureIgnoreCase)
                ? TransactionTypes.Withdraw
                : TransactionTypes.Deposit;
        }
    }
}
