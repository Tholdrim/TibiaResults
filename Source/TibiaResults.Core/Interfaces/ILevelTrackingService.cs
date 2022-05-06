namespace TibiaResults.Core
{
    internal interface ILevelTrackingService
    {
        void UpdateLevelTracker(ILevelTracker levelTracker, IEnumerable<HighscoreEntry>? oldHighscore, IEnumerable<HighscoreEntry>? newHighscore);
    }
}
