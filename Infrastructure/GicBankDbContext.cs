using Domain;
using Domain.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class GicBankDbContext(DbContextOptions<GicBankDbContext> options) : DbContext(options)
    {
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<AccountHolder> AccountHolder { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<InterestRules> InterestRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountsConfiguration());
            modelBuilder.ApplyConfiguration(new AccountHolderConfigurations());
            modelBuilder.ApplyConfiguration(new InterestRulesConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionsConfigurations());
            base.OnModelCreating(modelBuilder);
        }

        // To Do
        // Add on Model Creating Db using SqlLite to be Portable

    }
}
