namespace BlogHub.Data.Blogs.List.Helpers.Search;

public sealed record SearchDto
{
    public required string SearchQuery { get; init; }
    public required string SearchProperties { get; init; }
}