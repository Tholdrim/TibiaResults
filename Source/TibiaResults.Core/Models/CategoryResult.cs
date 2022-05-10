namespace TibiaResults.Core
{
    public record CategoryResult(IReadOnlyCollection<CategoryResultEntry> Entries)
    {
        public virtual bool IsAvailable => true;

        public bool IsEmpty => !Entries.Any();
    }
}
