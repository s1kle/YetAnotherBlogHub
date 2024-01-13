namespace BlogHub.Data.Blogs.List.All;

public sealed record GetAllBlogsQuery : IRequest<BlogListVm>
{
    public required int Page { get; init; }
    public required int Size { get; init; }
}