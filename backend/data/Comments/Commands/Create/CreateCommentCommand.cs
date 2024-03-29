namespace BlogHub.Data.Comments.Create;

public sealed record CreateCommentCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid ArticleId { get; init; }
    public required string Content { get; init; }
}