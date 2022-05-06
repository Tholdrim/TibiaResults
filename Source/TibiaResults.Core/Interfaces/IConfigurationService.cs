namespace TibiaResults.Core
{
    public interface IConfigurationService
    {
        Uri? BlobContainerUri { get; }

        IEnumerable<string> Characters { get; }

        (DateOnly From, DateOnly To) Dates { get; }
    }
}
