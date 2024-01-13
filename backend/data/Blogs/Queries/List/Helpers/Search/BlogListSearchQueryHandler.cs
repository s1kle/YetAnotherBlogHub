using System.Collections;
using System.Reflection;

namespace BlogHub.Data.Blogs.List.Helpers.Search;

internal sealed class BlogListSearchQueryHandler : IRequestHandler<BlogListSearchQuery, BlogListVm>
{
    private readonly BindingFlags _flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

    public async Task<BlogListVm> Handle(BlogListSearchQuery request, CancellationToken cancellationToken)
    {
        var blogs = await Task.FromResult(request.Blogs.Blogs);

        var filter = request.Query;
        var properties = request.Properties;

        if (string.IsNullOrWhiteSpace(filter)) return request.Blogs;


        if (properties is null || properties.Length == 0)
            properties = blogs
                .FirstOrDefault()?
                .GetType()
                .GetProperties()
                .Select(property => property.Name)
                .ToArray();

        return properties is null
            ? request.Blogs
            : new()
            {
                Blogs = blogs
                    .Where(blog => properties
                        .Any(property => ContainsQuery(blog, property, filter)))
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