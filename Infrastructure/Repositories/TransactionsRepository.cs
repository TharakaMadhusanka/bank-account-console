﻿using Domain.DomainModels;
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
            var isExist = await _context.Transactions
                .AnyAsync(x => x.TransactionNumber.StartsWith(transactionDate));

            return isExist
                    ? await _context.Transactions
                        .Where(x => x.TransactionNumber.StartsWith(transactionDate))
                        .MaxAsync(x => Convert.ToInt32(x.TransactionNumber.Substring(10, 2)))
                    : 0;
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
                    TransactionDate = y.CreatedDate.ToString("yyyyMMdd"),
                    EndBalance = y.EndBalance,

                })
                .ToListAsync();
        }
    }
}
