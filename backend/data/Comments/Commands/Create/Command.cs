namespace BlogHub.Data.Comments.Create;

public sealed record Command : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid BlogId { get; init; }
    public required string Content { get; init; }
}