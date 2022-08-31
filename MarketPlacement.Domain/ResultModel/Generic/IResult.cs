namespace MarketPlacement.Domain.ResultModel.Generic;

public interface IResult<out TData> : IResult
{
    public TData Data { get; }
}