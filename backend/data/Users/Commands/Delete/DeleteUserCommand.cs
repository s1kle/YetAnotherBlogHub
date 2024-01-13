namespace BlogHub.Data.Users.Delete;

public sealed record DeleteUserCommand : IRequest<Guid>
{
    public required Guid Id { get; init; }
}