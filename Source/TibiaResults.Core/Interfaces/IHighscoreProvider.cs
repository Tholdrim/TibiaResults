namespace TibiaResults.Core
{
    internal interface IHighscoreProvider
    {
        Task<Highscore?> GetHighscoreAsync(string identifier, DateOnly date);
    }
}
