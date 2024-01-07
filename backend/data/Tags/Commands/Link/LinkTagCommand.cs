using MediatR;

namespace BlogHub.Data.Tags.Commands.Link;

public class LinkTagCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid TagId { get; init; }
    public required Guid BlogId { get; init; }
}