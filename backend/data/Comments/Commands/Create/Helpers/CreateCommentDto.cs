namespace BlogHub.Data.Comments.Create;

public sealed record CreateCommentDto
{
    public required string Content { get; init; }
}