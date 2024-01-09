using System.Security.Claims;
using BlogHub.Data.Blogs.Queries.Get;
using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Blogs.Queries.ListAddUser;
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

    protected async Task<BlogListVm> ApplyFilters(BlogListVm blogs, ListSortDto? sortDto, ListSearchDto? searchDto)
    {
        var result = blogs;

        if (searchDto?.SearchQuery is not null)
        {
            
            var searchQuery = new ListSearchQuery()
            {
                Blogs = blogs,
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
                Blogs = blogs,
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

    protected async Task<BlogListWithAuthorVm> AddAuthors(BlogListVm blogs) =>
        await Mediator.Send(new ListAddUserQuery() { Blogs = blogs });

    protected async Task<BlogWithAuthorVm> AddAuthor(BlogVm blog) =>
        await Mediator.Send(new BlogAddUserQuery() { Blog = blog });
}