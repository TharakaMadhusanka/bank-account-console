using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class GicBankContext(DbContextOptions<GicBankContext> options) : DbContext(options)
    {
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<AccountHolder> AccountHolder { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<InterestRules> InterestRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accounts>();
            modelBuilder.Entity<AccountHolder>();
            modelBuilder.Entity<Transactions>();
            modelBuilder.Entity<InterestRules>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
