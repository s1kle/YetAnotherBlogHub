namespace BlogHub.Data.Blogs.Create.Helpers;

public sealed record CreateBlogDto
{
    public required string Title { get; init; }
    public string? Details { get; init; }
}