namespace BlogHub.Data.Articles.Get;

public sealed record GetArticleQuery : IRequest<ArticleVm>
{
    public required Guid Id { get; init; }
}