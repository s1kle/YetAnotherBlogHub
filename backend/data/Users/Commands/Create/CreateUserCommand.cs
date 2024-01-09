using MediatR;

namespace BlogHub.Data.Users.Commands.Create;

public record CreateUserCommand : IRequest<Guid>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}