namespace BlogHub.Data.Blogs.Commands.Create;

public sealed record CreateBlogCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required string Title { get; init; }
    public string? Details { get; init; }
}