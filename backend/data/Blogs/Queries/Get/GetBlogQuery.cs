namespace BlogHub.Data.Blogs.Queries.Get;

public sealed record GetBlogQuery : IRequest<BlogVm>
{
    public required Guid Id { get; init; }
}