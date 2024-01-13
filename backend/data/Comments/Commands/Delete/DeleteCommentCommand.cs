namespace BlogHub.Data.Comments.Delete;

public sealed record DeleteCommentCommand : IRequest<Guid>
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
}