using System.Collections;
using System.Reflection;
using BlogHub.Data.Blogs.Queries.GetList;

namespace BlogHub.Data.Blogs.Queries.ListSearch;

internal sealed class ListSearchQueryHandler : IRequestHandler<ListSearchQuery, BlogListVm>
{
    private readonly BindingFlags _flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

    public async Task<BlogListVm> Handle(ListSearchQuery request, CancellationToken cancellationToken)
    {
        var blogs = await Task.FromResult(request.Blogs.Blogs);

        var query = request.Query;
        var properties = request.Properties;

        if (string.IsNullOrWhiteSpace(query)) return request.Blogs;


        if (properties is null || properties.Length == 0)
            properties = blogs
                .FirstOrDefault()?
                .GetType()
                .GetProperties()
                .Select(property => property.Name)
                .ToArray();

        return properties is null
            ? request.Blogs
            : new () 
            {
                Blogs = blogs
                    .Where(blog => properties.Any(property => ContainsQuery(blog, property, query)))
                    .ToList()
            };
    }

    private bool ContainsQuery<T>(T obj, string propertyName, string query)
    {
        var property = obj?.GetType().GetProperty(propertyName, _flags);
        if (property == null) return false;

        var value = property.GetValue(obj);
        if (value == null) return false;
        
        if (value.GetType().IsValueType || value is string) return value
            .ToString()?
            .Contains(query)
            ?? false;

        if (value is IEnumerable collection) return collection
            .Cast<object>()
            .Any(item => item
                .GetType()
                .GetProperties(_flags)
                .Any(p => ContainsQuery(item, p.Name, query)));

        return value
            .GetType()
            .GetProperties(_flags)
            .Any(p => ContainsQuery(value, p.Name, query));
    }
}