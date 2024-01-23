using System.Reflection;

namespace BlogHub.Data.Articles.List.Helpers.Sort;

internal sealed class ArticleListSortQueryHandler : IRequestHandler<ArticleListSortQuery, ArticleListVm>
{
    public async Task<ArticleListVm> Handle(ArticleListSortQuery request, CancellationToken cancellationToken)
    {
        var Articles = await Task.FromResult(request.Articles.Articles);

        if (string.IsNullOrWhiteSpace(request.Property)) return request.Articles;

        var type = typeof(ArticleListItemVm);

        var property = type.GetProperty(request.Property,
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.IgnoreCase);

        if (property is null) return request.Articles;

        return new()
        {
            Articles = (request.Descending
                ? Articles.OrderByDescending(property.GetValue)
                : Articles.OrderBy(property.GetValue))
                .ToList()
        };
    }
}