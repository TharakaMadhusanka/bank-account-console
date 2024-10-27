using Domain.Constants;
using Domain.Dao;
using Domain.Dto;
using Domain.GicDomainInterface;
using Domain.Interfaces;
using System.Globalization;
using System.Linq.Expressions;

namespace Domain.GicDomainServices
{
    public class InterestRuleDomainService : IInterestRuleDomainService
    {
        private readonly IInterestRuleRepository _interestRuleRepository;
        public InterestRuleDomainService(IInterestRuleRepository interestRuleRepository)
        {
            _interestRuleRepository = interestRuleRepository;
        }

        public async Task<IReadOnlyCollection<InterestRuleDto>> InterestRuleDefineRequestHandleAsync(InterestRuleDefineRequestParamsDao interestRuleDefineRequestParamsDao)
        {
            // 1. Existing Rule -> Update to new value
            // 2. Not Existing Rule -> Create New Rule
            // 3. Return all Rules
            DateTime.TryParseExact(interestRuleDefineRequestParamsDao.Date, CommonConstant.TransactionDateInputFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateToUse);
            var interestRule = new InterestRules
            {
                RuleId = interestRuleDefineRequestParamsDao.Rule,
                AnnualRate = Decimal.Parse(interestRuleDefineRequestParamsDao.InterestRate),
                EffectiveFromDate = dateToUse,
                EffectiveToDate = DateTime.MaxValue,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Id = Guid.NewGuid(),
            };
            Expression<Func<InterestRules, bool>> expression = x => (x.RuleId == interestRule.RuleId && x.EffectiveFromDate.Date == dateToUse.Date);
            await _interestRuleRepository.UpsertInterestRuleAsync(expression, interestRule);
            // Return all interest rules
            return await _interestRuleRepository.RetrieveAllInterestRulesAsync();
        }
    }
}
