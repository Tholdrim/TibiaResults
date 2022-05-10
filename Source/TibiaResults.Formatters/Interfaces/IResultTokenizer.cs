using TibiaResults.Core;

namespace TibiaResults.Formatters
{
    internal interface IResultTokenizer
    {
        LinkedList<Token> Tokenize(IResult result);
    }
}
