using TibiaResults.Core;

namespace TibiaResults.Formatters
{
    internal static class TokenizerHelper
    {
        public static IDictionary<string, int> DetermineCategoryWinners(IEnumerable<CategoryResultEntry> entries)
        {
            var groupedEntriesWithBestProgress = entries
                .Where(e => e.Progress.GetValueOrDefault(0) > 0)
                .GroupBy(e => e.Progress)
                .OrderByDescending(e => e.Key)
                .Take(3);

            return groupedEntriesWithBestProgress
                .SelectMany((g, i) => g.Select(e => new { e.Name, Place = i + 1 }))
                .ToDictionary(o => o.Name, o => o.Place);
        }

        public static string FormatProgress(long? progress, bool isApproximate)
        {
            if (!progress.HasValue || progress.Value == 0)
            {
                return string.Empty;
            }

            var formattedProgress = progress.Value.ToSignedNumberString();

            return isApproximate ? $"{Messages.Approximately} {formattedProgress}" : formattedProgress;
        }

        public static string FormatValue(long value, bool isApproximate)
        {
            var formattedValue = value.ToNumberString();

            return isApproximate ? $"{Messages.Approximately} {formattedValue}" : formattedValue;
        }

        public static IReadOnlyCollection<string> GetIcons(CategoryResultEntry entry, IDictionary<string, int> categoryWinners)
        {
            categoryWinners.TryGetValue(entry.Name, out var place);

            var icons = new[]
            {
                GetPlaceIcon(place),
                GetNewIcon(entry)
            };

            return icons.OfType<string>().ToList();
        }

        private static string? GetNewIcon(CategoryResultEntry entry) => entry switch
        {
            { Progress: null }                 => UnicodeIcons.SquaredNew,
            { IsApproximate: true, Rank: { } } => UnicodeIcons.SquaredNew,
            _                                  => null
        };

        private static string? GetPlaceIcon(int place) => place switch
        {
            1 => UnicodeIcons.FirstPlaceMedal,
            2 => UnicodeIcons.SecondPlaceMedal,
            3 => UnicodeIcons.ThirdPlaceMedal,
            _ => null
        };
    }
}
