using System.Text.Json.Serialization;

namespace TibiaResults.Core
{
    internal class HighscoreRoot
    {
        [JsonPropertyName("highscores")]
        public Highscore? Highscores { get; init; }
    }
}
