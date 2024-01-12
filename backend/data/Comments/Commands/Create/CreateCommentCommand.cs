namespace BlogHub.Data.Comments.Commands.Create;

public sealed record CreateCommentCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid BlogId { get; init; }
    public required string Content { get; init; }
}