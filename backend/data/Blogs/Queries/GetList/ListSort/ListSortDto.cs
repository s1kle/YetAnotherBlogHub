namespace BlogHub.Data.Blogs.Queries.ListSort;

public record ListSortDto
{
    public required string SortProperty { get; init; }
    public required string SortDirection { get; init; }
}