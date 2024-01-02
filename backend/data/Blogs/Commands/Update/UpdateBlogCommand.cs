using MediatR;

namespace BlogHub.Data.Blogs.Commands.Update;

public record UpdateBlogCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string? Details { get; init; }
}