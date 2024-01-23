namespace BlogHub.Data.Articles.List.Helpers.Sort;

public sealed record ArticleListSortQuery : IRequest<ArticleListVm>
{
    public required ArticleListVm Articles { get; init; }
    public required string Property { get; init; }
    public bool Descending { get; init; }
}