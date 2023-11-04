namespace BlogHub.Data.Commands.Update;

public record UpdateBlogDto
{
    public required string Title { get; init; }
    public required string? Details { get; init; }
}