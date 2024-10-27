namespace Domain.DomainModels
{
    public class Accounts
    {
        public Guid Id { get; set; }
        public required string AccountNo { get; set; }
        public Guid AccountHolderId { get; set; }
        public decimal TotalBalance { get; set; }
        public DateTime LastTransactionDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public required string AccountType { get; set; }
        public required AccountHolder AccountHolder { get; set; }
    }
}
