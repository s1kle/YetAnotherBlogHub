namespace BlogHub.Data.Blogs.Queries.GetList;

public record TagsFilter
{
    public required string[] Tags { get; init; }
}