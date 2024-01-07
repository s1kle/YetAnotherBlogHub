using MediatR;

namespace BlogHub.Data.Blogs.Commands.Delete;

public record DeleteBlogCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid Id { get; init; }
}