using TibiaResults.Core;

namespace TibiaResults.Formatters
{
    internal class ResultTokenizer : IResultTokenizer
    {
        public LinkedList<Token> Tokenize(IResult result)
        {
            var tokens = result.Categories
                .OrderBy(c => c.Order)
                .SelectMany(c => TokenizeCategoryPart(c, result[c]));

            return new LinkedList<Token>(tokens);
        }

        private static IEnumerable<Token> TokenizeCategoryPart(Category category, CategoryResult categoryResult)
        {
            var categoryToken = new CategoryToken(category.Name);
            var categoryResultTokens = TokenizeCategoryResult(categoryResult);

            return categoryResultTokens.Prepend(categoryToken);
        }

        private static IEnumerable<Token> TokenizeCategoryResult(CategoryResult categoryResult) => categoryResult switch
        {
            { IsAvailable: false } => new[] { new MessageToken(UnicodeIcons.ConstructionSign, Messages.NotAvailable) },
            { IsEmpty: true }      => new[] { new MessageToken(UnicodeIcons.OpenFileFolder, Messages.NoEntries) },
            _                      => TokenizeCategoryResultEntries(categoryResult.Entries)
        };

        private static IEnumerable<Token> TokenizeCategoryResultEntries(IEnumerable<CategoryResultEntry> entries)
        {
            var categoryWinners = TokenizerHelper.DetermineCategoryWinners(entries);

            var rankedEntries = entries.Where(e => e.Rank.HasValue).OrderBy(e => e.Rank);
            var unrankedEntries = entries.Where(e => !e.Rank.HasValue).OrderByDescending(e => e.Value);

            foreach (var entry in rankedEntries.Concat(unrankedEntries))
            {
                var icons = TokenizerHelper.GetIcons(entry, categoryWinners);
                var progress = TokenizerHelper.FormatProgress(entry.Progress, entry.IsApproximate);
                var value = TokenizerHelper.FormatValue(entry.Value, entry.IsApproximate);

                yield return entry.Rank.HasValue
                    ? new RankedEntryToken(entry.Rank.Value, entry.Name, value, progress, icons)
                    : new UnrankedEntryToken(entry.Name, value, progress, icons);
            }
        }
    }
}
