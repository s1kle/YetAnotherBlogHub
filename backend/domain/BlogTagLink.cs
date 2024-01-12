namespace BlogHub.Domain;

public sealed record BlogTagLink
{
    public required Guid Id { get; init; }
    public required Guid BlogId { get; init; }
    public required Guid TagId { get; init; }
}