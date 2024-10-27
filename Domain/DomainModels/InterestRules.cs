using System.ComponentModel.DataAnnotations;

namespace Domain.DomainModels
{
    public class InterestRules
    {
        public Guid Id { get; set; }
        [Range(0, 100)]
        public decimal AnnualRate { get; set; }
        public required string RuleId { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
