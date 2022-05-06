using System.Text.Json.Serialization;

namespace TibiaResults.Core
{
    internal class Highscore
    {
        [JsonPropertyName("highscore_list")]
        public IEnumerable<HighscoreEntry>? HighscoreList { get; init; }
    }
}
