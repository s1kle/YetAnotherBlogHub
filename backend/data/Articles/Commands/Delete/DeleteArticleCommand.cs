namespace BlogHub.Data.Articles.Delete;

public sealed record DeleteArticleCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid Id { get; init; }
}