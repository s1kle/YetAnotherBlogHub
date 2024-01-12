namespace BlogHub.Data.Blogs.List.Helpers.Search;

public sealed record Dto
{
    public required string SearchQuery { get; init; }
    public required string SearchProperties { get; init; }
}