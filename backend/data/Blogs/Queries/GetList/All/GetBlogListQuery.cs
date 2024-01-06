using MediatR;

namespace BlogHub.Data.Blogs.Queries.GetList;

public record GetBlogListQuery : IRequest<BlogListVm>
{
    public required int Page { get; init; }
    public required int Size { get; init; }
}