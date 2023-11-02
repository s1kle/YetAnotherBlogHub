using MediatR;

namespace BlogHub.Data.Queries.GetBlog;

public record GetBlogQuery : IRequest<BlogVm>
{
    public required Guid UserId { get; init; }
    public required Guid Id { get; init; }
}