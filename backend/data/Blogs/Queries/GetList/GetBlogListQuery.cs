using MediatR;

namespace BlogHub.Data.Queries.GetList;

public record GetBlogListQuery : IRequest<BlogListVm>
{
    public required Guid UserId { get; init; }
}