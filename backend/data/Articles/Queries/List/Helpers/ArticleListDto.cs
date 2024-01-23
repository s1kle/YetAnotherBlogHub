using BlogHub.Data.Articles.List.Helpers.Search;
using BlogHub.Data.Articles.List.Helpers.Sort;

namespace BlogHub.Data.Articles.List.Helpers;

public sealed record ArticleListDto
{
    public required ListDto List { get; init; }
    public SearchDto? Search { get; init; }
    public SortDto? Sort { get; init; }
}