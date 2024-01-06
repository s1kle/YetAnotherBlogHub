using System.Security.Claims;
using BlogHub.Api.Extensions;
using BlogHub.Data.Blogs.Queries.GetList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

public abstract class BaseBlogController : ControllerBase
{
    protected IMediator Mediator { get; }
    protected Guid UserId =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    protected BaseBlogController(IMediator mediator) =>
        Mediator = mediator;

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