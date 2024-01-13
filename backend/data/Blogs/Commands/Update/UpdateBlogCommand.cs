namespace BlogHub.Data.Blogs.Update;

public sealed record UpdateBlogCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public string? Details { get; init; }
}