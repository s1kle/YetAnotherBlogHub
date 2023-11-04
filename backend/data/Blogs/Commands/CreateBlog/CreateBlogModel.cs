namespace BlogHub.Data.Commands.CreateBlog;

public record CreateBlogModel
{
    public required string Title { get; init; }
    public required string? Details { get; init; }
}