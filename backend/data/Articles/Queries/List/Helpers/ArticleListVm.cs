namespace BlogHub.Data.Articles.List.Helpers;

public sealed record ArticleListVm
{
    public required IReadOnlyList<ArticleListItemVm> Articles { get; init; }
}