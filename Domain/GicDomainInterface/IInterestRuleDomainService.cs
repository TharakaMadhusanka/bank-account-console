using Domain.Dao;
using Domain.Dto;

namespace Domain.GicDomainInterface
{
    public interface IInterestRuleDomainService
    {
        Task<IReadOnlyCollection<InterestRuleDto>> InterestRuleDefineRequestHandleAsync(InterestRuleDefineRequestParamsDao interestRuleDefineRequestParamsDao);
    }
}
