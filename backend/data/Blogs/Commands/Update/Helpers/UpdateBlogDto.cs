namespace BlogHub.Data.Blogs.Update.Helpers;

public sealed record UpdateBlogDto
{
    public required string Title { get; init; }
    public string? Details { get; init; }
}