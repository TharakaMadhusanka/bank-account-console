using BankConsole;
using BankConsoleApplication;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
#region Inject DB Context for DI
services.AddDbContext<GicBankDbContext>(options =>
    options.UseSqlServer(
        connectionString: "Data Source=localhost\\SQLEXPRESS;Initial Catalog=gicbank;Integrated Security=True;TrustServerCertificate=True",
        sqlServerOptionsAction: options => options.MigrationsAssembly("Infrastructure")
    ));
#endregion

#region Service Injection
// Singleton, cannot be used as it will create single instance and will not be reflected the new instance changes
// Add Scoped will create object within the requested scope will be sufficient for this scenario
services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
services.AddTransient<IInterestRuleRepository, InterestRuleRepository>();
services.AddSingleton<IConsoleInterfaceService, ConsoleInterfaceService>();

#endregion

// DI Container
var serviceContainer = services.BuildServiceProvider();

// intialize trigger Console Application
var console = serviceContainer.GetRequiredService<IConsoleInterfaceService>();
await console.StartAsync();

