namespace BlogHub.Data.Queries.GetBlogList;

public record BlogVmForList
{
    public required string Title { get; init; }
    public required string? Details { get; init; }
}