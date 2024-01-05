namespace BlogHub.Domain;

public record BlogTag
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required Guid BlogId { get; init; }
    public required Guid TagId { get; init; }
}