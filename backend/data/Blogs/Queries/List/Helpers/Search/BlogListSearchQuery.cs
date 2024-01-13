namespace BlogHub.Data.Blogs.List.Helpers.Search;

public sealed record BlogListSearchQuery : IRequest<BlogListVm>
{
    public required BlogListVm Blogs { get; init; }
    public required string Query { get; init; }
    public required string[] Properties { get; init; }
}