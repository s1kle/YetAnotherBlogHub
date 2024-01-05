using System.Reflection;

namespace BlogHub.Data.Blogs.Queries.GetList;

public static class IEnumerableExtensions
{
    public static IEnumerable<T> SortByProperty<T>(this IEnumerable<T> query, SortFilter? sortFilter)
    {
        if (sortFilter is null) return query;

        if (string.IsNullOrWhiteSpace(sortFilter.SortProperty)) return query;

        var entityType = typeof(T);

        var property = entityType
            .GetProperty(sortFilter.SortProperty,
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.IgnoreCase);
        
        if (property is null) return query;

        return sortFilter.SortDescending
            ? query.OrderByDescending(entity => property.GetValue(entity))
            : query.OrderBy(entity => property.GetValue(entity));
    }

    public static IEnumerable<T> Search<T>(this IEnumerable<T> query, SearchFilter? searchFilter)
    {
        if (searchFilter is null) return query;

        if (string.IsNullOrWhiteSpace(searchFilter.SearchQuery) || searchFilter.SearchProperties.Length < 1) return query;

        var entityType = typeof(T);

        var properties = searchFilter.SearchProperties
            .Select(name => entityType
                .GetProperty(name,
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.IgnoreCase))
            .Where(property => property is not null);

        if (properties.Any() is false) return query;

        return query.Where(entity => properties.Any(property => property!
            .GetValue(entity)?
            .ToString()?
            .Contains(searchFilter.SearchQuery, StringComparison.OrdinalIgnoreCase)
            ?? false));
    }
}