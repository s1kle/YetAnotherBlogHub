namespace BlogHub.Data.Blogs.Queries.GetList;

public record SearchFilter
{
    public required string SearchQuery { get; init; }
    public required string[] SearchProperties { get; init; }
}