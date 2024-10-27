namespace Domain.DomainModels
{
    public class AccountHolder
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string IdentificationNo { get; set; }
        public string? ContactNo { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<Accounts> Accounts { get; } = [];
    }
}
