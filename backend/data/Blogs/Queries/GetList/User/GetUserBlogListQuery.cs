namespace BlogHub.Data.Blogs.Queries.GetList.User;

public sealed record GetUserBlogListQuery : IRequest<BlogListVm>
{
    public required Guid UserId { get; init; }
    public required int Page { get; init; }
    public required int Size { get; init; }
}