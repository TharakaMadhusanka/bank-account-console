using Domain;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class InterestRuleRepository(DbContext context) : GenericRepository<InterestRules>(context), IInterestRuleRepository
    {
    }
}
