namespace MarketPlacement.Domain.ResultModel;

public interface IResult
{
    IReadOnlyCollection<string> Messages { get; }

    bool Success { get; }

    Exception? Exception { get; }
}