namespace TibiaResults.Core
{
    internal interface IHighscoreRetrievalService
    {
        Task<IEnumerable<HighscoreEntry>?> GetOldHighscoreAsync(string identifier);

        Task<IEnumerable<HighscoreEntry>?> GetNewHighscoreAsync(string identifier);
    }
}
