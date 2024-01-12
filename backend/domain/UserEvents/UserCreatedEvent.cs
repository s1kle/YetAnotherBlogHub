namespace BlogHub.Domain.UserEvents;

public sealed record UserCreatedEvent()
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}