using System.Text;

namespace TibiaResults.Formatters
{
    internal class DiscordFormatter : PlainTextFormatter
    {
        public DiscordFormatter(IResultTokenizer resultTokenizer)
            : base(resultTokenizer)
        {
        }

        protected override void AppendCategory(StringBuilder stringBuilder, CategoryToken categoryToken, Token? previousToken)
        {
            if (previousToken != null)
            {
                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine($"**{categoryToken.Name}**");
            stringBuilder.AppendLine();
        }

        protected override void AppendCommonEntrySuffix(StringBuilder stringBuilder, string progress, IEnumerable<string> icons)
        {
            if (!string.IsNullOrEmpty(progress))
            {
                stringBuilder.Append($" (**{progress}**)");
            }

            if (icons.Any())
            {
                stringBuilder.Append(' ');
                stringBuilder.AppendJoin(' ', icons);
            }

            stringBuilder.AppendLine();
        }

        protected override void AppendMessage(StringBuilder stringBuilder, MessageToken messageToken, Token? previousToken)
        {
            stringBuilder.AppendLine($"{messageToken.Icon} *{messageToken.Message}*");
        }
    }
}
