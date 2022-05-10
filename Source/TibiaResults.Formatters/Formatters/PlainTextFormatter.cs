using System.Text;

namespace TibiaResults.Formatters
{
    internal class PlainTextFormatter : BaseFormatter
    {
        public PlainTextFormatter(IResultTokenizer resultTokenizer)
            : base(resultTokenizer)
        {
        }

        protected override void AppendCategory(StringBuilder stringBuilder, CategoryToken categoryToken, Token? previousToken)
        {
            if (previousToken != null)
            {
                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine(categoryToken.Name);
            stringBuilder.AppendLine();
        }

        protected virtual void AppendCommonEntrySuffix(StringBuilder stringBuilder, string progress, IEnumerable<string> icons)
        {
            if (!string.IsNullOrEmpty(progress))
            {
                stringBuilder.Append($" ({progress})");
            }

            if (icons.Any())
            {
                stringBuilder.Append(' ');
                stringBuilder.AppendJoin(string.Empty, icons);
            }

            stringBuilder.AppendLine();
        }

        protected override void AppendMessage(StringBuilder stringBuilder, MessageToken messageToken, Token? previousToken)
        {
            stringBuilder.AppendLine($"{messageToken.Icon} {messageToken.Message}");
        }

        protected override void AppendRankedEntry(StringBuilder stringBuilder, RankedEntryToken entryToken, Token? previousToken)
        {
            stringBuilder.Append($"{entryToken.Rank}. {entryToken.Name} - {entryToken.Value}");

            AppendCommonEntrySuffix(stringBuilder, entryToken.Progress, entryToken.Icons);
        }

        protected override void AppendUnrankedEntry(StringBuilder stringBuilder, UnrankedEntryToken entryToken, Token? previousToken)
        {
            if (previousToken is RankedEntryToken)
            {
                stringBuilder.AppendLine();
            }

            stringBuilder.Append($"{entryToken.Name} - {entryToken.Value}");

            AppendCommonEntrySuffix(stringBuilder, entryToken.Progress, entryToken.Icons);
        }
    }
}
