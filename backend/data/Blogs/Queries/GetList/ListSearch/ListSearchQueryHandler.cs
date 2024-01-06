using System.Reflection;
using BlogHub.Data.Blogs.Queries.GetList;
using MediatR;

namespace BlogHub.Data.Blogs.Queries.ListSearch;

public class ListSearchQueryHandler : IRequestHandler<ListSearchQuery, BlogListVm>
{
    public async Task<BlogListVm> Handle(ListSearchQuery request, CancellationToken cancellationToken)
    {
        var blogs = await Task.FromResult(request.Blogs.Blogs);

        if (string.IsNullOrWhiteSpace(request.Query)) return request.Blogs;

        var type = typeof(BlogVmForList);

        var properties = request.Properties
                .Select(name => type
                    .GetProperty(name,
                        BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.IgnoreCase))
                .Where(property => property is not null);

        if (properties.Any() is false) return request.Blogs;

        return new ()
        {
            Blogs = blogs
                .Where(blog => properties.Any(property => property!
                    .GetValue(blog)?
                    .ToString()?
                    .Contains(request.Query, StringComparison.OrdinalIgnoreCase)
                ?? false))
                .ToList()
        };
    }
}