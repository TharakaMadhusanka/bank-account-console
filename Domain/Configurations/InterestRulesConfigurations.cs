using Domain.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class InterestRulesConfiguration : IEntityTypeConfiguration<InterestRules>
    {
        public void Configure(EntityTypeBuilder<InterestRules> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);
            builder.Property(x => x.EffectiveToDate).IsRequired();
            builder.Property(x => x.EffectiveFromDate).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.UpdatedDate).IsRequired();
            builder.Property(x => x.RuleId).IsRequired();
            builder.Property(x => x.AnnualRate).IsRequired();
        }
    }
}
