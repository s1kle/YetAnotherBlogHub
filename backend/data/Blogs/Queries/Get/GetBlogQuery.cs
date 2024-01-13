namespace BlogHub.Data.Blogs.Get;

public sealed record GetBlogQuery : IRequest<BlogVm>
{
    public required Guid Id { get; init; }
}