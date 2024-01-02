using MediatR;

namespace BlogHub.Data.Tags.Commands.Create;

public record CreateTagCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid BlogId { get; init; }
    public required string Name { get; init; }
}