using BankConsole;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<IConsoleInterfaceService, ConsoleInterfaceService>();
// DI Container
var serviceContainer = services.BuildServiceProvider();

// intialize trigger Console Application
var console = serviceContainer.GetRequiredService<IConsoleInterfaceService>();
console.Start();

