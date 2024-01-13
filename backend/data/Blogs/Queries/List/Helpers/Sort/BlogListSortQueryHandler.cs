using System.Reflection;

namespace BlogHub.Data.Blogs.List.Helpers.Sort;

internal sealed class BlogListSortQueryHandler : IRequestHandler<BlogListSortQuery, BlogListVm>
{
    public async Task<BlogListVm> Handle(BlogListSortQuery request, CancellationToken cancellationToken)
    {
        var blogs = await Task.FromResult(request.Blogs.Blogs);

        if (string.IsNullOrWhiteSpace(request.Property)) return request.Blogs;

        var type = typeof(BlogListItemVm);

        var property = type.GetProperty(request.Property,
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.IgnoreCase);

        if (property is null) return request.Blogs;

        return new()
        {
            Blogs = (request.Descending
                ? blogs.OrderByDescending(property.GetValue)
                : blogs.OrderBy(property.GetValue))
                .ToList()
        };
    }
}