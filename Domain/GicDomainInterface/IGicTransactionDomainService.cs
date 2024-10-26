using Domain.Dao;
using Domain.Dto;

namespace Domain.GicDomainInterface
{
    public interface IGicTransactionDomainService
    {
        Task<InputTransactionResponseDto> InputTransactionHandleAsync(InputTransactionRequestParamsDao inputTransactionRequestParamsDao);
    }
}
