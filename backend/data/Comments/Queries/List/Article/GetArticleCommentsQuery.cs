namespace BlogHub.Data.Comments.List.Article;

public sealed record GetArticleCommentsQuery : IRequest<IReadOnlyList<CommentVm>>
{
    public required Guid ArticleId { get; init; }
}