using Domain.DomainModels;
using Domain.Dto;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IInterestRuleRepository : IGenericRepository<InterestRules>
    {
        public Task<IReadOnlyCollection<InterestRuleDto>> RetrieveAllInterestRulesAsync();
        Task UpsertInterestRuleAsync(Expression<Func<InterestRules, bool>> expression, InterestRules entity);
    }
}
