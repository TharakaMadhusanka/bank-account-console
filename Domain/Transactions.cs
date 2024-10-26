namespace Domain
{
    public class Transactions
    {
        public Guid Id { get; set; }
        public required string CodeTransactionType { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Amount { get; set; }
        public required string Source { get; set; }
        public required string TransactionNumber { get; set; }
        public Guid AccountId { get; set; }
        public required Accounts Account { get; set; }
    }
}
