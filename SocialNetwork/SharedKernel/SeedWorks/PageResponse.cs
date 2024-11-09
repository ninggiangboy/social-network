namespace SharedKernel.SeedWorks;

public class PageResponse<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int TotalPages { get; set; }
    public long TotalItems { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }

    public bool HasNext => Page < TotalPages;

    private PageResponse()
    {
    }

    public static PageResponse<T> Create(IEnumerable<T> items, long totalItems, int page, int limit)
    {
        return new PageResponse<T>
        {
            Items = items,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)limit),
            Page = page,
            Limit = limit
        };
    }
}