using FluentValidation;

namespace Domain.Validators
{
    public class InterestRuleValidator : AbstractValidator<InterestRules>
    {
        public InterestRuleValidator()
        {
            RuleFor(rule => rule.AnnualRate)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100);

            RuleFor(rule => rule.RuleId)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(rule => rule.EffectiveFromDate)
                .NotEmpty()
                .LessThanOrEqualTo(rule => rule.EffectiveToDate);

            RuleFor(rule => rule.EffectiveToDate)
                .GreaterThanOrEqualTo(rule => rule.EffectiveFromDate);
        }
    }
}
