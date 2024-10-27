using Domain.DomainModels;
using Domain.Dto;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transactions>
    {
        Task<int> GetLatestRunNoSequenceAsync(string transactionDate);
        IQueryable<Transactions> GetTransactionsByExpressionAsync(Expression<Func<Transactions, bool>> expression);
        Task<IReadOnlyCollection<TransactionRecordDto>> GetTransactionsPrintStatementByExpressionAsync(Expression<Func<Transactions, bool>> expression);
    }
}
