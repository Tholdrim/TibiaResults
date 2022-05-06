namespace TibiaResults.Core
{
    internal class Result : Dictionary<Category, CategoryResult>, IResultDraft
    {
        private Result()
        {
        }

        IEnumerable<Category> IResult.Categories => Keys;

        public static IResultDraft CreateNew() => new Result();
    }
}
