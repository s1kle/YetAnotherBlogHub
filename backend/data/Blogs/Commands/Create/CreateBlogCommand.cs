using MediatR;

namespace BlogHub.Data.Blogs.Commands.Create;

public record CreateBlogCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required string Title { get; init; }
    public required string? Details { get; init; }
}