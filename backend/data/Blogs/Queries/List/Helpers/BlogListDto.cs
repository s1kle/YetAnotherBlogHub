using BlogHub.Data.Blogs.List.Helpers.Search;
using BlogHub.Data.Blogs.List.Helpers.Sort;

namespace BlogHub.Data.Blogs.List.Helpers;

public sealed record BlogListDto
{
    public required ListDto List { get; init; }
    public SearchDto? Search { get; init; }
    public SortDto? Sort { get; init; }
}