namespace BlogHub.Domain.UserEvents;

public record UserDeletedEvent()
{
    public required Guid Id { get; init; }
}