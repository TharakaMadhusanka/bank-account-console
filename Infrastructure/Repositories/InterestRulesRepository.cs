using Domain;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class InterestRuleRepository(GicBankDbContext context) : GenericRepository<InterestRules>(context), IInterestRuleRepository
    {
    }
}
