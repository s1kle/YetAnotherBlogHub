namespace BlogHub.Domain;

public sealed record Comment
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required Guid ArticleId { get; init; }
    public required DateTime CreationDate { get; init; }
    public required string Content { get; init; }
}