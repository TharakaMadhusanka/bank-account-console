using Domain.DomainModels;
using Domain.Dto;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class InterestRuleRepository(GicBankDbContext context) : GenericRepository<InterestRules>(context), IInterestRuleRepository
    {
        private readonly GicBankDbContext _context = context;

        public async Task<IReadOnlyCollection<InterestRuleDto>> RetrieveAllInterestRulesAsync()
        {
            return await _context.InterestRules.AsNoTracking()
                .OrderBy(x => x.EffectiveFromDate)
                .Select(x => new InterestRuleDto
                {
                    Date = x.EffectiveFromDate.Date.ToString("yyyyMMdd"),
                    RuleId = x.RuleId,
                    Rate = x.AnnualRate.ToString()
                })
                .ToListAsync();
        }

        public async Task UpsertInterestRuleAsync(Expression<Func<InterestRules, bool>> expression, InterestRules entity)
        {
            var existingEntity = await _context.InterestRules.FirstOrDefaultAsync(expression);

            if (existingEntity == null)
            {
                await _context.AddAsync(entity);
            }
            else
            {
                existingEntity.AnnualRate = entity.AnnualRate;
                existingEntity.UpdatedDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }
    }
}
