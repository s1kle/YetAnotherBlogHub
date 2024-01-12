namespace BlogHub.Data.Users.Delete;

public sealed record Command : IRequest<Guid>
{
    public required Guid Id { get; init; }
}