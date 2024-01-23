using System.Collections;
using System.Reflection;

namespace BlogHub.Data.Articles.List.Helpers.Search;

internal sealed class ArticleListSearchQueryHandler : IRequestHandler<ArticleListSearchQuery, ArticleListVm>
{
    private readonly BindingFlags _flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

    public async Task<ArticleListVm> Handle(ArticleListSearchQuery request, CancellationToken cancellationToken)
    {
        var Articles = await Task.FromResult(request.Articles.Articles);

        var filter = request.Query;
        var properties = request.Properties;

        if (string.IsNullOrWhiteSpace(filter)) return request.Articles;


        if (properties is null || properties.Length == 0)
            properties = Articles
                .FirstOrDefault()?
                .GetType()
                .GetProperties()
                .Select(property => property.Name)
                .ToArray();

        return properties is null
            ? request.Articles
            : new()
            {
                Articles = Articles
                    .Where(Article => properties
                        .Any(property => ContainsQuery(Article, property, filter)))
                    .ToList()
            };
    }

    private bool ContainsQuery<T>(T obj, string propertyName, string filter)
    {
        var property = obj?.GetType().GetProperty(propertyName, _flags);
        if (property == null) return false;

        var value = property.GetValue(obj);
        if (value == null) return false;

        if (value.GetType().IsValueType || value is string) return value
            .ToString()?
            .Contains(filter)
            ?? false;

        if (value is IEnumerable collection) return collection
            .Cast<object>()
            .Any(item => item
                .GetType()
                .GetProperties(_flags)
                .Any(p => ContainsQuery(item, p.Name, filter)));

        return value
            .GetType()
            .GetProperties(_flags)
            .Any(p => ContainsQuery(value, p.Name, filter));
    }
}