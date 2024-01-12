namespace BlogHub.Domain;

public sealed record Tag
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
}
