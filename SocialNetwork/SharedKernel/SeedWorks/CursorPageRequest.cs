namespace SharedKernel.SeedWorks;

public class CursorPageRequest
{
    public int Limit { get; init; }
    public string? SortBy { get; init; }
    public bool IsAscending { get; init; } = true;
    public string? NextToken { get; init; }
}