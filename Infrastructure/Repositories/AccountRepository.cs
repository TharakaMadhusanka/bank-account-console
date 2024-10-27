using Domain.DomainModels;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AccountRepository(GicBankDbContext context) : GenericRepository<Accounts>(context), IAccountRepository
    {
        private readonly GicBankDbContext _context = context;

        public async Task<Accounts?> GetAccountByAccountNoAsync(string accountNo)
        {
            return await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNo == accountNo);
        }
    }
}
