namespace SharedKernel.SeedWorks;

public class CursorPageResponse<T>
{
    public ICollection<T> Items { get; init; } = null!;
    public int Limit { get; init; }
    public bool HasNext { get; init; }
    public string SortBy { get; init; } = null!;
    public string? NextToken { get; init; }

    public static CursorPageResponse<T> Create(ICollection<T> items, int limit, string sortBy, string? nextToken)
    {
        return new CursorPageResponse<T>
        {
            Items = items,
            Limit = limit,
            HasNext = items.Count == limit,
            SortBy = sortBy,
            NextToken = nextToken
        };
    }
}