namespace BlogHub.Data.Articles.List.All;

public sealed record GetAllArticlesQuery : IRequest<ArticleListVm>
{
    public required int Page { get; init; }
    public required int Size { get; init; }
}