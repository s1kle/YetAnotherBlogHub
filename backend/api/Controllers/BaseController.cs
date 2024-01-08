using System.Security.Claims;
using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Blogs.Queries.ListSearch;
using BlogHub.Data.Blogs.Queries.ListSort;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

[Route("api")]
public abstract class BaseController : ControllerBase
{
    protected IMediator Mediator { get; }
    protected Guid UserId =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    protected BaseController(IMediator mediator) =>
        Mediator = mediator;

    protected async Task<BlogListVm> ApplyFilters(BlogListVm list, ListSortDto? sortDto, ListSearchDto? searchDto)
    {
        var result = list;

        if (searchDto?.SearchQuery is not null)
        {
            
            var searchQuery = new ListSearchQuery()
            {
                Blogs = list,
                Query = searchDto.SearchQuery,
                Properties = searchDto.SearchProperties?.Split(' ')
                    ?? new [] { "title" }
            };

            result = await Mediator.Send(searchQuery);
        }
        

        if (sortDto?.SortProperty is not null)
        {
            var sortQuery = new ListSortQuery()
            {
                Blogs = list,
                Property = sortDto.SortProperty,
                Descending = sortDto.SortDirection switch
                {
                    "desc" => true,
                    _ => false
                }
            };

            result = await Mediator.Send(sortQuery);
        }

        return result;
    }
}