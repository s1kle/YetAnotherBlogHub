namespace BlogHub.Domain;

public sealed record ArticleTagLink
{
    public required Guid Id { get; init; }
    public required Guid ArticleId { get; init; }
    public required Guid TagId { get; init; }
}