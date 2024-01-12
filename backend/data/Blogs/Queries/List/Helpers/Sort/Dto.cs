namespace BlogHub.Data.Blogs.List.Helpers.Sort;

public sealed record Dto
{
    public required string SortProperty { get; init; }
    public required string SortDirection { get; init; }
}