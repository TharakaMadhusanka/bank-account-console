using Domain;
using Domain.Dto;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class TransactionsRepository(GicBankDbContext context) : GenericRepository<Transactions>(context), ITransactionRepository
    {
        private readonly GicBankDbContext _context = context;
        public async Task<int> GetLatestRunNoSequenceAsync(string transactionDate)
        {
            return await _context.Transactions
                .Where(x => x.TransactionNumber.StartsWith(transactionDate))
                .MaxAsync(x => Convert.ToInt32(x.TransactionNumber.Substring(10, 2)));
        }

        public IQueryable<Transactions> GetTransactionsByExpressionAsync(Expression<Func<Transactions, bool>> expression)
        {
            return _context.Transactions.Where(expression);
        }

        public async Task<IReadOnlyCollection<TransactionRecordDto>> GetTransactionsPrintStatementByExpressionAsync(Expression<Func<Transactions, bool>> expression)
        {
            return await _context.Transactions
                .Where(expression)
                .OrderBy(x => x.CreatedDate)
                .Select(y => new TransactionRecordDto
                {
                    Amount = y.Amount,
                    TransactionNo = y.TransactionNumber,
                    TransactionType = y.CodeTransactionType,
                    TransactionDate = y.CreatedDate.ToString("yyyyMMdd")

                })
                .ToListAsync();
        }
    }
}
