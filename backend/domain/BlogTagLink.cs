namespace BlogHub.Domain;

public record BlogTagLink
{
    public required Guid Id { get; init; }
    public required Guid BlogId { get; init; }
    public required Guid TagId { get; init; }
}