using MediatR;

namespace BlogHub.Data.Tags.Commands.Delete;

public record DeleteTagCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid Id { get; init; }
}