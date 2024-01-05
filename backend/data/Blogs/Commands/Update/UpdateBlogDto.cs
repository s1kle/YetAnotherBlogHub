namespace BlogHub.Data.Commands.Update;

public record UpdateBlogDto
{
    public required string Title { get; init; }
    public string? Details { get; init; }
}