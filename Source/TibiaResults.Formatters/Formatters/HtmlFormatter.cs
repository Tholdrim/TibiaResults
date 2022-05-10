using System.Text;

namespace TibiaResults.Formatters
{
    internal class HtmlFormatter : BaseFormatter
    {
        public HtmlFormatter(IResultTokenizer resultTokenizer)
            : base(resultTokenizer)
        {
        }

        protected override void AppendCategory(StringBuilder stringBuilder, CategoryToken categoryToken, Token? previousToken)
        {
            if (previousToken is RankedEntryToken or UnrankedEntryToken)
            {
                stringBuilder.AppendLine("</ol>");
            }

            stringBuilder.AppendLine($"<h2>{categoryToken.Name}</h2>");
        }

        protected override void AppendMessage(StringBuilder stringBuilder, MessageToken messageToken, Token? previousToken)
        {
            stringBuilder.AppendLine($"<p style=\"font-style: italic;\">{messageToken.Icon} {messageToken.Message}</p>");
        }

        protected override void AppendRankedEntry(StringBuilder stringBuilder, RankedEntryToken entryToken, Token? previousToken)
        {
            if (previousToken is CategoryToken)
            {
                stringBuilder.Append("<ol>");
            }

            stringBuilder.Append($"<li value=\"{entryToken.Rank}\">{entryToken.Name} - {entryToken.Value}");

            AppendCommonEntrySuffix(stringBuilder, entryToken.Progress, entryToken.Icons);
        }

        protected override void AppendUnrankedEntry(StringBuilder stringBuilder, UnrankedEntryToken entryToken, Token? previousToken)
        {
            if (previousToken is RankedEntryToken)
            {
                stringBuilder.AppendLine("</ol>");
            }

            if (previousToken is CategoryToken or RankedEntryToken)
            {
                stringBuilder.Append("<ol style=\"list-style: none;\">");
            }

            stringBuilder.Append($"<li>{entryToken.Name} - {entryToken.Value}");

            AppendCommonEntrySuffix(stringBuilder, entryToken.Progress, entryToken.Icons);
        }

        private static void AppendCommonEntrySuffix(StringBuilder stringBuilder, string progress, IEnumerable<string> icons)
        {
            if (!string.IsNullOrEmpty(progress))
            {
                stringBuilder.Append($" (<span style=\"font-weight: bold;\">{progress}</span>)");
            }

            if (icons.Any())
            {
                stringBuilder.Append(' ');
                stringBuilder.AppendJoin(string.Empty, icons);
            }

            stringBuilder.Append("</li>");
        }
    }
}
