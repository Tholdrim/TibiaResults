namespace TibiaResults.Core
{
    public interface IResult
    {
        CategoryResult this[Category category] { get; }

        IEnumerable<Category> Categories { get; }
    }
}
