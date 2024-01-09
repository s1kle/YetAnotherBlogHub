using BlogHub.Data.Blogs.Queries.Get;
using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Blogs.Queries.ListAddUser;
using BlogHub.Data.Blogs.Queries.ListSearch;
using BlogHub.Data.Blogs.Queries.ListSort;
using BlogHub.Data.Tags.Queries.GetList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

public sealed class UnauthorizeBlogController : BaseController
{
    public UnauthorizeBlogController(IMediator mediator) : base (mediator) { }

    [HttpGet("blogs")]
    public async Task<ActionResult<BlogListWithAuthorVm>> GetAll([FromQuery] GetListDto dto,
        [FromQuery] ListSortDto? sortDto = null, [FromQuery] ListSearchDto? searchDto = null)
    {
        var query = new GetBlogListQuery()
        {
            Page = dto.Page,
            Size = dto.Size
        };

        var result = await Mediator.Send(query);

        result = await ApplyFilters(result, sortDto, searchDto);

        var response = await AddAuthors(result);

        return Ok(response);
    }

    [HttpGet("blog/{id}")]
    public async Task<ActionResult<BlogWithAuthorVm>> Get(Guid id)
    {
        var query = new GetBlogQuery() { Id = id };

        var blog = await Mediator.Send(query);

        var response = await AddAuthor(blog);

        return Ok(response);
    }

    [HttpGet("blog/{id}/tags")]
    public async Task<ActionResult<TagListVm>> GetTags(Guid id)
    {
        var query = new GetBlogTagListQuery()
        {
            BlogId = id
        };

        var response = await Mediator.Send(query);

        return Ok(response);
    }
}