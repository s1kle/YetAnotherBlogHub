namespace BlogHub.Data.Tags.List.Article;

public sealed record GetArticleTagsQuery : IRequest<IReadOnlyList<TagVm>>
{
    public required Guid ArticleId { get; init; }
}