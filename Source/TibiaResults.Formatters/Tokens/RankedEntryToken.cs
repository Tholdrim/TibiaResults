namespace TibiaResults.Formatters
{
    internal record RankedEntryToken(int Rank, string Name, string Value, string Progress, IEnumerable<string> Icons) : Token;
}
