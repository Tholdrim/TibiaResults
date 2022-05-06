namespace TibiaResults.Core
{
    internal interface ILevelTracker
    {
        Levels GetCharacterLevels(string character);

        void Update(string character, Levels characterLevels);
    }
}
