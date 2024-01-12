namespace BlogHub.Data.Blogs.Queries.ListSort;

public sealed record ListSortDto
{
    public required string SortProperty { get; init; }
    public required string SortDirection { get; init; }
}