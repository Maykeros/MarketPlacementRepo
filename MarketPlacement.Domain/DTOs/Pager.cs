namespace MarketPlacement.Domain.DTOs;

public class Pager<TData>
{
    public Pager(IReadOnlyCollection<TData> data, int totalCount)
    {
        Data = data;
        TotalCount = totalCount;
    }
    public IReadOnlyCollection<TData> Data { get; init; }

    public int TotalCount { get; init; }
}