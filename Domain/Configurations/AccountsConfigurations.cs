using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class AccountsConfiguration : IEntityTypeConfiguration<Accounts>
    {
        public void Configure(EntityTypeBuilder<Accounts> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.AccountHolderId);
            builder.HasOne(x => x.AccountHolder);
            builder.Property(x => x.AccountHolderId).IsRequired();
            builder.Property(x => x.LastTransactionDate).IsRequired();
            builder.Property(x => x.AccountType).IsRequired().HasMaxLength(20);
            builder.Property(x => x.AccountNo).IsRequired().HasMaxLength(50);
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.UpdatedDate).IsRequired();
        }
    }
}
