namespace Domain.Dto
{
    public class InputTransactionResponseDto
    {
        private readonly IList<TransactionRecordDto>? _transactionRecordList;
        public InputTransactionResponseDto() { }
        public InputTransactionResponseDto(IReadOnlyCollection<TransactionRecordDto> transactionsList)
        {
            _transactionRecordList = [.. transactionsList];
        }
        public IReadOnlyCollection<TransactionRecordDto>? TransactionRecordsList => _transactionRecordList?.ToList().AsReadOnly();
        public bool IsTransactionSuccess { get; set; }
        public required string AccountNo { get; set; }
        public CommonErrorDto? CommonError { get; set; }
    }
}
