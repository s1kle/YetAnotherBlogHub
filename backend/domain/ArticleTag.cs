namespace BlogHub.Domain;

public sealed record ArticleTag
{
    public required Guid Id { get; init; }
    public required Guid ArticleId { get; init; }
    public required Guid TagId { get; init; }
}