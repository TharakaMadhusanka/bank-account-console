namespace Domain.Dto
{
    public class TransactionRecordDto
    {
        public required string TransactionNo { get; set; }
        public required decimal Amount { get; set; }
        public required string TransactionType { get; set; }
        public required string TransactionDate { get; set; }
    }
}
