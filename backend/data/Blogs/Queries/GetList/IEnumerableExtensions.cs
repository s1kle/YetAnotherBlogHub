using System.Reflection;

namespace BlogHub.Data.Queries.GetList;

public static class IEnumerableExtensions
{
    public static IEnumerable<T> SortByProperty<T>(this IEnumerable<T> query, string? propertyName, bool descending)
    {
        if (string.IsNullOrWhiteSpace(propertyName)) return query;

        var entityType = typeof(T);
        var property = entityType
            .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
        
        if (property is null) return query;

        return descending
            ? query.OrderByDescending(entity => property.GetValue(entity))
            : query.OrderBy(entity => property.GetValue(entity));
    }

    public static IEnumerable<T> Search<T>(this IEnumerable<T> query, string? searchQuery, string[]? propertyNames)
    {
        if (string.IsNullOrWhiteSpace(searchQuery) || propertyNames is null || propertyNames.Length < 1) return query;

        var entityType = typeof(T);
        var properties = propertyNames
            .Select(name => entityType
                .GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase))
            .Where(property => property is not null);

        if (properties.Any() is false) return query;

        return query.Where(entity => properties.Any(property => property!
            .GetValue(entity)?.ToString()?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ?? false));
    }
}