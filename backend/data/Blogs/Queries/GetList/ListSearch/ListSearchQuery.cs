using BlogHub.Data.Blogs.Queries.GetList;
using MediatR;

namespace BlogHub.Data.Blogs.Queries.ListSearch;

public record ListSearchQuery : IRequest<BlogListVm>
{
    public required BlogListVm Blogs { get; init; }
    public required string Query { get; init; }
    public required string[] Properties { get; init; }
}