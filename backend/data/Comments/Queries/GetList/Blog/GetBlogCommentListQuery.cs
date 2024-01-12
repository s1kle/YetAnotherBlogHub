namespace BlogHub.Data.Comments.Queries.GetList.Blog;

public sealed record GetBlogCommentListQuery : IRequest<IReadOnlyList<CommentVm>>
{
    public required Guid BlogId { get; init; }
}