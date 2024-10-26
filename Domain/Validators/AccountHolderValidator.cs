using FluentValidation;

namespace Domain.Validators
{
    public class AccountHolderValidator : AbstractValidator<AccountHolder>
    {
        public AccountHolderValidator()
        {
            RuleFor(accountHolder => accountHolder.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(accountHolder => accountHolder.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(accountHolder => accountHolder.IdentificationNo)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(accountHolder => accountHolder.ContactNo)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(accountHolder => accountHolder.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(accountHolder => accountHolder.CreatedDate)
                .NotEmpty();

            RuleFor(accountHolder => accountHolder.UpdatedDate)
                .NotEmpty();
        }
    }
}
