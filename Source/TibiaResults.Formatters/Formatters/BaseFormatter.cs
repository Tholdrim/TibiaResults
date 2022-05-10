using System.Text;
using TibiaResults.Core;

namespace TibiaResults.Formatters
{
    internal abstract class BaseFormatter : IResultFormatter
    {
        private readonly IResultTokenizer _resultTokenizer;

        public BaseFormatter(IResultTokenizer resultTokenizer)
        {
            _resultTokenizer = resultTokenizer;
        }

        public string FormatResult(IResult result)
        {
            var stringBuilder = new StringBuilder();
            var tokens = _resultTokenizer.Tokenize(result);

            for (var currentNode = tokens.First; currentNode != null; currentNode = currentNode.Next)
            {
                var tokenFormatter = GetTokenFormatter(currentNode.Value);

                tokenFormatter(stringBuilder, currentNode.Previous?.Value);
            }

            return stringBuilder.ToString();
        }

        protected abstract void AppendCategory(StringBuilder stringBuilder, CategoryToken categoryToken, Token? previousToken);

        protected abstract void AppendMessage(StringBuilder stringBuilder, MessageToken messageToken, Token? previousToken);

        protected abstract void AppendRankedEntry(StringBuilder stringBuilder, RankedEntryToken entryToken, Token? previousToken);

        protected abstract void AppendUnrankedEntry(StringBuilder stringBuilder, UnrankedEntryToken entryToken, Token? previousToken);

        private Action<StringBuilder, Token?> GetTokenFormatter(Token token) => token switch
        {
            CategoryToken categoryToken   => (stringBuilder, previousToken) => AppendCategory(stringBuilder, categoryToken, previousToken),
            MessageToken messageToken     => (stringBuilder, previousToken) => AppendMessage(stringBuilder, messageToken, previousToken),
            RankedEntryToken entryToken   => (stringBuilder, previousToken) => AppendRankedEntry(stringBuilder, entryToken, previousToken),
            UnrankedEntryToken entryToken => (stringBuilder, previousToken) => AppendUnrankedEntry(stringBuilder, entryToken, previousToken),
            _                             => throw new ArgumentOutOfRangeException(nameof(token), "Unknown token type")
        };
    }
}
