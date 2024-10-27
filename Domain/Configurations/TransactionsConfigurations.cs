using Domain.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class TransactionsConfigurations : IEntityTypeConfiguration<Transactions>
    {
        public void Configure(EntityTypeBuilder<Transactions> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);
            builder.Property(x => x.CodeTransactionType).IsRequired();
            builder.Property(x => x.TransactionNumber).IsRequired();
            builder.Property(x => x.AccountId).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
        }
    }
}
