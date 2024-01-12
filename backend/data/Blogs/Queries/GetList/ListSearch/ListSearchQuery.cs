using BlogHub.Data.Blogs.Queries.GetList;

namespace BlogHub.Data.Blogs.Queries.ListSearch;

public sealed record ListSearchQuery : IRequest<BlogListVm>
{
    public required BlogListVm Blogs { get; init; }
    public required string Query { get; init; }
    public required string[] Properties { get; init; }
}