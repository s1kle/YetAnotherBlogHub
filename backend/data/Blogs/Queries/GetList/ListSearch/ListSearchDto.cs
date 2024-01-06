namespace BlogHub.Data.Blogs.Queries.ListSearch;

public record ListSearchDto
{
    public required string SearchQuery { get; init; }
    public required string SearchProperties { get; init; }
}