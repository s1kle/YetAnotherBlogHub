namespace BlogHub.Data.Users.Commands.Delete;

public sealed record DeleteUserCommand : IRequest<Guid>
{
    public required Guid Id { get; init; }
}