namespace BlogHub.Data.Tags.Commands.Unlink;

public sealed class UnlinkTagCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid TagId { get; init; }
    public required Guid BlogId { get; init; }
}