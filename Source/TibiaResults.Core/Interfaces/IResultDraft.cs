namespace TibiaResults.Core
{
    internal interface IResultDraft : IResult
    {
        void Add(Category category, CategoryResult categoryResult);
    }
}
