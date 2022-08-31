namespace MarketPlacement.Application.Extensions;

public static class PagerExtension
{
    public static IQueryable<T> GetPage<T>(this IQueryable<T> all,int page, int count)
    {
        return all.Skip((page - 1) * count).Take(count);
    }
}