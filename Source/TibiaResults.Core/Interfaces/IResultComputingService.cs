namespace TibiaResults.Core
{
    public interface IResultComputingService
    {
        Task<IResult> GetResultAsync();
    }
}
