using Domain;
using System.Globalization;

namespace BankConsoleApplication.DataSeed
{
    internal class DataSeed
    {
        public ICollection<InterestRules> GetInterestRules()
        {
            var dateRanges = GenerateDateRanges(2024);
            int index = 0;
            ICollection<InterestRules> interestRules = [];
            var random = new Random();

            foreach (var range in dateRanges)
            {
                index++;
                interestRules.Add(new()
                {
                    Id = Guid.NewGuid(),
                    RuleId = $"RULE0{index.ToString(CultureInfo.InvariantCulture)}",
                    AnnualRate = random.Next(1, 5),
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    EffectiveFromDate = range.Item1,
                    EffectiveToDate = range.Item2
                });
            }
            return [.. interestRules.OrderBy(x => x.EffectiveFromDate)];
        }

        private static IEnumerable<(DateTime, DateTime)> GenerateDateRanges(int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31);

            var currentStartDate = startDate;
            while (currentStartDate <= endDate)
            {
                var currentEndDate = currentStartDate.AddDays(13);
                if (currentEndDate > endDate)
                {
                    currentEndDate = endDate;
                }

                yield return (currentStartDate, currentEndDate);
                currentStartDate = currentEndDate.AddDays(1);
            }
        }
    }
}
