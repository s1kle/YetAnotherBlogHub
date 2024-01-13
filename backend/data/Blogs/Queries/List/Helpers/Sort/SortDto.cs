namespace BlogHub.Data.Blogs.List.Helpers.Sort;

public sealed record SortDto
{
    public required string SortProperty { get; init; }
    public required string SortDirection { get; init; }
}