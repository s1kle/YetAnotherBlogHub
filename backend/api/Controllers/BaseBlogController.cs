using BlogHub.Data.Blogs.Queries.GetList;
using MediatR;

namespace BlogHub.Api.Controllers;

public abstract class BaseBlogController : BlogHubController
{
    protected BaseBlogController(IMediator mediator) : base(mediator) { }

    protected static SortFilter? GetSortFilter(string? property, string? direction)
    {
        if (property is null) return null;

        return new ()
        {
            SortProperty = property,
            SortDescending = direction?.Equals("desc") ?? false
        };
    }

    protected static SearchFilter? GetSearchFilter(string? searchQuery, string? searchProperties)
    {
        if (searchQuery is null || searchProperties is null) return null;

        return new ()
        {
            SearchQuery = searchQuery,
            SearchProperties = searchProperties.Split(' ')
        };
    }
}