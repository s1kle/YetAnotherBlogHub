namespace BlogHub.Data.Commands.Create;

public record CreateBlogDto
{
    public required string Title { get; init; }
    public string? Details { get; init; }
}