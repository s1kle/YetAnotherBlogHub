namespace BlogHub.Data.Tags.Link;

public sealed class LinkTagCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid TagId { get; init; }
    public required Guid ArticleId { get; init; }
}