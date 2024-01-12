namespace BlogHub.Data.Tags.Unlink;

public sealed class Command : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid TagId { get; init; }
    public required Guid BlogId { get; init; }
}