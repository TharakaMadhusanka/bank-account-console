
using BankConsole;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BankConsoleApplication
{
    internal class ConsoleInterfaceService : IConsoleInterfaceService
    {
        private readonly IInterestRuleRepository _interestRuleRepository;
        private readonly IServiceProvider _serviceProvider;
        public ConsoleInterfaceService(IInterestRuleRepository interestRuleRepository, IServiceProvider serviceProvider)
        {
            _interestRuleRepository = interestRuleRepository;
            _serviceProvider = serviceProvider;
        }
        public async Task StartAsync()
        {
            Console.WriteLine("Print Menu");
            // Seed Data to the System
            // 1. InterestRules
            // Interest Rule is applied,
            await SeedDataAsync();

            var interestRatesList = await _interestRuleRepository.GetAllAsync();
            foreach (var interestRates in interestRatesList) Console.WriteLine(interestRates.RuleId, interestRates.EffectiveFromDate.ToString(),
                interestRates.EffectiveToDate.ToString());
            Console.ReadKey();
        }

        // This is not functional part of the Banking System
        // But as for the Demo and Assignment Purpose added this Data Seeding
        private async Task SeedDataAsync()
        {
            if (await _interestRuleRepository.ExistAsync()) { return; }
            var dataSeed = new DataSeed.DataSeed();
            var context = _serviceProvider.GetRequiredService<GicBankDbContext>();
            await context.Database.EnsureCreatedAsync();
            await context.InterestRules.AddRangeAsync(dataSeed.GetInterestRules());
            await context.SaveChangesAsync();
        }
    }
}
