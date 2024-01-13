namespace BlogHub.Data.Users.Create;

public sealed record CreateUserCommand : IRequest<Guid>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}