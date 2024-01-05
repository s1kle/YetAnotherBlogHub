namespace BlogHub.Data.Blogs.Queries.GetList;

public record GetListDto
{
    public required int Page { get; init; }
    public required int Size { get; init; }
    public string? SearchQuery { get; init; }
    public string? SearchProperties { get; init; }
    public string? SortProperty { get; init; }
    public string? SortDirection { get; init; }
}