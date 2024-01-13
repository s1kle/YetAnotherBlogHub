namespace BlogHub.Data.Comments.List.Blog;

public sealed record GetBlogCommentsQuery : IRequest<IReadOnlyList<CommentVm>>
{
    public required Guid BlogId { get; init; }
}