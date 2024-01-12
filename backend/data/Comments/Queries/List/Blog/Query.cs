namespace BlogHub.Data.Comments.List.Blog;

public sealed record Query : IRequest<IReadOnlyList<CommentVm>>
{
    public required Guid BlogId { get; init; }
}