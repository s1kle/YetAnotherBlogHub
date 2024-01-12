namespace BlogHub.Domain;

public sealed record User
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
