using BlogHub.Data.Blogs.Queries.GetList;
using MediatR;

namespace BlogHub.Data.Blogs.Queries.ListSort;

public record ListSortQuery : IRequest<BlogListVm>
{
    public required BlogListVm Blogs { get; init; }
    public required string Property { get; init; }
    public bool Descending { get; init; }
}