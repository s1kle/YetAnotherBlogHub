namespace BlogHub.Domain.UserEvents;

public sealed record UserDeletedEvent()
{
    public required Guid Id { get; init; }
}