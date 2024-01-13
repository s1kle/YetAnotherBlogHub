namespace BlogHub.Data.Blogs.List.User;

public sealed record GetUserBlogsQuery : IRequest<BlogListVm>
{
    public required Guid UserId { get; init; }
    public required int Page { get; init; }
    public required int Size { get; init; }
}