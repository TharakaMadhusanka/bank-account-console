using Domain.Interfaces;
using GicConsole.Interface;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace GicConsole
{
    internal class ConsoleInterfaceService : IConsoleInterfaceService
    {
        private readonly IInterestRuleRepository _interestRuleRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGicConsoleService _gicConsoleService;

        public ConsoleInterfaceService(IInterestRuleRepository interestRuleRepository,
            IServiceProvider serviceProvider,
            IGicConsoleService gicConsoleService)
        {
            _interestRuleRepository = interestRuleRepository;
            _serviceProvider = serviceProvider;
            _gicConsoleService = gicConsoleService;
        }
        public async Task StartAsync()
        {
            // Seed Data to the System
            // 1. InterestRules
            // Interest Rule is applied,
            await SeedDataAsync();

            // 1. Welcome Prompt and Init Transactions Flow
            await _gicConsoleService.WelcomeAndInitTransactionsAsync();

        }

        // This is not functional part of the Banking System
        // But as for the Demo and Assignment Purpose added this Data Seeding
        private async Task SeedDataAsync()
        {
            if (await _interestRuleRepository.AnyAsync()) { return; }
            var dataSeed = new DataSeed.DataSeed();
            var context = _serviceProvider.GetRequiredService<GicBankDbContext>();
            await context.Database.EnsureCreatedAsync();
            await context.InterestRules.AddRangeAsync(dataSeed.GetInterestRules());
            await context.SaveChangesAsync();
        }
    }
}
