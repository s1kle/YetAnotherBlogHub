namespace BlogHub.Data.Comments.Commands.Create;

public sealed record CreateCommentDto
{
    public required string Content { get; init; }
}