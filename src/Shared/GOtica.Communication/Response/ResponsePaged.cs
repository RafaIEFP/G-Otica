namespace GOtica.Communication.Response;

public record ResponsePaged<T>
{
    public IReadOnlyCollection<T> Items { get; init; } = [];
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
}
