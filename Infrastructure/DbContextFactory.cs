using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<GicBankDbContext>
    {
        // Design Time Migration
        public GicBankDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GicBankDbContext>();
            optionsBuilder.UseSqlServer(
                connectionString: "Server=localhost\\SQLEXPRESS;Database=gicbank;Integrated Security=True;TrustServerCertificate=True",
                sqlServerOptionsAction: options => options.MigrationsAssembly("Infrastructure"));

            return new GicBankDbContext(optionsBuilder.Options);
        }
    }
}
