using MediatR;

namespace BlogHub.Data.Queries.GetBlogList;

public record GetBlogListQuery : IRequest<BlogListVm>
{
    public required Guid UserId { get; init; }
}