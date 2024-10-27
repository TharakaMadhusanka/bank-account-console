using Domain.GicDomainInterface;
using Domain.GicDomainServices;
using Domain.Interfaces;
using GicConsole;
using GicConsole.Interface;
using GicConsole.Services;
using GicConsole.Validators;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

#region Inject DB Context for DI
services.AddDbContext<GicBankDbContext>(options =>
    options.UseSqlServer(
        connectionString: "Data Source=localhost\\SQLEXPRESS;Initial Catalog=gicbank;Integrated Security=True;TrustServerCertificate=True",
        sqlServerOptionsAction: options => options.MigrationsAssembly("Infrastructure")
    ), ServiceLifetime.Singleton);
#endregion

#region Service Injection
// Singleton, cannot be used as it will create single instance and will not be reflected the new instance changes
// Add Scoped will create object within the requested scope will be sufficient for this scenario
services.AddSingleton<IConsoleInterfaceService, ConsoleInterfaceService>();
services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
services.AddScoped<IGicConsoleService, GicConsoleService>();
services.AddTransient<IInputValidator, InputValidator>();

// Domain Services
services.AddScoped<ITransactionRepository, TransactionsRepository>();
services.AddScoped<IAccountRepository, AccountRepository>();
services.AddScoped<IAccountHolderRepository, AccountHolderRepository>();
services.AddScoped<IGicTransactionDomainService, GicTransactionDomainService>();
services.AddScoped<IInterestRuleDomainService, InterestRuleDomainService>();
services.AddScoped<IInterestRuleRepository, InterestRuleRepository>();
services.AddScoped<IAccountStatementDomainService, AccountStatementDomainService>();
#endregion

// DI Container
var serviceContainer = services.BuildServiceProvider();

// intialize trigger Console Application
var console = serviceContainer.GetRequiredService<IConsoleInterfaceService>();
await console.StartAsync();

