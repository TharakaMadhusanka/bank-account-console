using BankConsole;
using BankConsoleApplication;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var services = new ServiceCollection();
services.AddSingleton<IConsoleInterfaceService, ConsoleInterfaceService>();

#region Inject DB Context
var folder = Environment.SpecialFolder.LocalApplicationData;
var path = Environment.GetFolderPath(folder);
var dbFileLocation = Path.Join(path, "gicbank.db");
services.AddDbContext<GicBankContext>(options =>
        options.UseSqlite($"Data Source={dbFileLocation}", option =>
        {
            option.MigrationsAssembly(assemblyName: Assembly.GetExecutingAssembly().FullName);
            option.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
        }));
#endregion

#region Service Injection
// Singleton, cannot be used as it will create single instance and will not be reflected the new instance changes
// Add Scoped will create object within the requested scope will be sufficient for this scenario
services.AddScoped<IInterestRuleRepository, InterestRuleRepository>();
#endregion

// DI Container
var serviceContainer = services.BuildServiceProvider();

// intialize trigger Console Application
var console = serviceContainer.GetRequiredService<IConsoleInterfaceService>();
await console.StartAsync();

