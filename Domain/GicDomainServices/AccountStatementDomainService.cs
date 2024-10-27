using Domain.Dao;
using Domain.DomainModels;
using Domain.Dto;
using Domain.GicDomainInterface;
using Domain.Interfaces;
using System.Linq.Expressions;

namespace Domain.GicDomainServices
{
    public class AccountStatementDomainService : IAccountStatementDomainService
    {
        private readonly ITransactionRepository _transactionRepository;
        public AccountStatementDomainService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IReadOnlyCollection<TransactionRecordDto>> AccountStatementHandleAsync(AccountStatementInputRequestDao accountStatementInputRequestDao)
        {
            // 1. Check the end of the month for the given date, end of month or passed
            // 2. If end of month, calculate transaction and inserted in to Transaction Table, Update Balance
            // 3. Print the statement
            // Interest Rules is bit unclear, thus as I understood, I will do the calculation as below
            // Transaction Date falling in the given period [14 days, 11 days etc.] => Not Really clear :(
            Expression<Func<Transactions, bool>> predicate = x => x.TransactionNumber.StartsWith(accountStatementInputRequestDao.YearMonth);
            return await _transactionRepository.GetTransactionsPrintStatementByExpressionAsync(predicate);

        }
    }
}
