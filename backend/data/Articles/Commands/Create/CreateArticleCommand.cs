namespace BlogHub.Data.Articles.Create;

public sealed record CreateArticleCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required string Title { get; init; }
    public string? Details { get; init; }
}