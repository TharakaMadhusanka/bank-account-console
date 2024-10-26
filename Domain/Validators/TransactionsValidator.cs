using FluentValidation;

namespace Domain.Validators
{
    internal class TransactionsValidator : AbstractValidator<Transactions>
    {
        public TransactionsValidator()
        {
            RuleFor(transaction => transaction.CodeTransactionType)
                .NotEmpty()
                .MaximumLength(2);

            RuleFor(transaction => transaction.Amount)
                .GreaterThan(0);

            RuleFor(transaction => transaction.Source)
                .NotEmpty();

            RuleFor(transaction => transaction.TransactionNumber)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(transaction => transaction.AccountId)
                .NotEmpty();
        }
    }
}
