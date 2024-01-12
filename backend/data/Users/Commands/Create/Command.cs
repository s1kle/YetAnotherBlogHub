namespace BlogHub.Data.Users.Create;

public sealed record Command : IRequest<Guid>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}