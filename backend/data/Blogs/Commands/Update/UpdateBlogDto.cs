namespace BlogHub.Data.Blogs.Commands.Update;

public sealed record UpdateBlogDto
{
    public required string Title { get; init; }
    public string? Details { get; init; }
}