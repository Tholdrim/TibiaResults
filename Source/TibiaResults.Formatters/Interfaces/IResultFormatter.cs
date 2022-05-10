using TibiaResults.Core;

namespace TibiaResults.Formatters
{
    public interface IResultFormatter
    {
        string FormatResult(IResult result);
    }
}
