namespace BlogHub.Domain;

public record Tag
{
    public required Guid Id { get; init; }
    public required Guid BlogId { get; init; }
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
}
