namespace BlogHub.Data.BlogTags.Commands.Create;

public record CreateBlogTagDto
{
    public required Guid TagId { get; init; }
}