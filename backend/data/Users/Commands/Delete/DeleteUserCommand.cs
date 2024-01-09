using MediatR;

namespace BlogHub.Data.Users.Commands.Delete;

public record DeleteUserCommand : IRequest<Guid>
{
    public required Guid Id { get; init; }
}