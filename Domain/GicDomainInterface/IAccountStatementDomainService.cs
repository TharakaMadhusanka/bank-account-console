using Domain.Dao;
using Domain.Dto;

namespace Domain.GicDomainInterface
{
    public interface IAccountStatementDomainService
    {
        Task<IReadOnlyCollection<TransactionRecordDto>> AccountStatementHandleAsync(AccountStatementInputRequestDao accountStatementInputRequestDao);
    }
}
