using System.Reflection;
using BlogHub.Data.Blogs.Queries.GetList;
using MediatR;

namespace BlogHub.Data.Blogs.Queries.ListSort;

public class ListSortQueryHandler : IRequestHandler<ListSortQuery, BlogListVm>
{
    public async Task<BlogListVm> Handle(ListSortQuery request, CancellationToken cancellationToken)
    {
        var blogs = await Task.FromResult(request.Blogs.Blogs);

        if (string.IsNullOrWhiteSpace(request.Property)) return request.Blogs;

        var type = typeof(BlogVmForList);

        var property = type.GetProperty(request.Property,
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.IgnoreCase);

        if (property is null) return request.Blogs;

        return new ()
        {
            Blogs = (request.Descending
                ? blogs.OrderByDescending(property.GetValue)
                : blogs.OrderBy(property.GetValue))
                .ToList()
        };
    }
}