namespace BlogHub.Data.Blogs.Queries.GetList.All;

public sealed record GetBlogListQuery : IRequest<BlogListVm>
{
    public required int Page { get; init; }
    public required int Size { get; init; }
}