namespace BlogHub.Data.Blogs.Create;

public sealed record Command : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required string Title { get; init; }
    public string? Details { get; init; }
}