namespace BlogHub.Data.Blogs.Delete;

public sealed record Command : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid Id { get; init; }
}