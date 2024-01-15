namespace BlogHub.Data.Articles.Update;

public sealed record UpdateArticleCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public string? Details { get; init; }
}