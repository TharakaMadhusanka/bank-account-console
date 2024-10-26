using Domain;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class InterestRuleRepository(GicBankDbContext bankDbContext) : GenericRepository<InterestRules>(bankDbContext), IInterestRuleRepository
    {
    }
}
