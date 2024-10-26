namespace Domain
{
    public class InterestRules
    {
        public Guid Id { get; set; }
        public decimal AnnualRate { get; set; }
        public required string RuleId { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
