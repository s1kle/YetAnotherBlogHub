namespace BlogHub.Domain.UserEvents;

public record UserCreatedEvent()
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}