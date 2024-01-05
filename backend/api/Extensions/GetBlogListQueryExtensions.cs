using BlogHub.Data.Queries.GetList;

namespace BlogHub.Api.Extensions;

public static class GetBlogListQueryExtensions
{
    public static GetBlogListQuery ApplySortFilter(this GetBlogListQuery query, string? property, string? direction)
    {
        if (property is null) return query;

        return query with
        {
            SortFilter = new ()
                {
                    SortProperty = property,
                    SortDescending = direction switch
                    {
                        "desc" => true,
                        _ => false
                    }
                }
        };
    }

    public static GetBlogListQuery ApplySearchFilter(this GetBlogListQuery query, string? searchQuery, string? searchProperties)
    {
        if (searchQuery is null || searchProperties is null) return query;
        return query = query with
        {
            SearchFilter = new ()
            {
                SearchQuery = searchQuery,
                SearchProperties = searchProperties.Split(' ')
            }
        };
    }
}