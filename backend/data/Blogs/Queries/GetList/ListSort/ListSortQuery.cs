using BlogHub.Data.Blogs.Queries.GetList;

namespace BlogHub.Data.Blogs.Queries.ListSort;

public sealed record ListSortQuery : IRequest<BlogListVm>
{
    public required BlogListVm Blogs { get; init; }
    public required string Property { get; init; }
    public bool Descending { get; init; }
}