using FluentValidation;

namespace Domain.Validators
{
    public class AccountValidator : AbstractValidator<Accounts>
    {
        public AccountValidator()
        {
            RuleFor(account => account.AccountNo)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(account => account.AccountHolderId)
                .NotEmpty();

            RuleFor(account => account.TotalBalance)
                .GreaterThanOrEqualTo(0);

            RuleFor(account => account.LastTransactionDate)
                .NotEmpty();

            RuleFor(account => account.CreatedDate)
                .NotEmpty();

            RuleFor(account => account.UpdatedDate)
                .NotEmpty();
        }
    }
}
