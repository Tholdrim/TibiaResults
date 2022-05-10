namespace TibiaResults.Formatters
{
    internal record UnrankedEntryToken(string Name, string Value, string Progress, IEnumerable<string> Icons) : Token;
}
