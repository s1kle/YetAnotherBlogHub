namespace BlogHub.Data.Blogs.List.Helpers.Sort;

public sealed record BlogListSortQuery : IRequest<BlogListVm>
{
    public required BlogListVm Blogs { get; init; }
    public required string Property { get; init; }
    public bool Descending { get; init; }
}