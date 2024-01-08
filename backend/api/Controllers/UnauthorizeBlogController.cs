using BlogHub.Data.Blogs.Queries.Get;
using BlogHub.Data.Blogs.Queries.GetList;
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
    public async Task<ActionResult<BlogListVm>> GetAll([FromQuery] GetListDto dto,
        [FromQuery] ListSortDto? sortDto = null, [FromQuery] ListSearchDto? searchDto = null)
    {
        var query = new GetBlogListQuery()
        {
            Page = dto.Page,
            Size = dto.Size
        };

        var response = await Mediator.Send(query);

        response = await ApplyFilters(response, sortDto, searchDto);

        return Ok(response);
    }

    [HttpGet("blog/{id}")]
    public async Task<ActionResult<BlogVm>> Get(Guid id)
    {
        var query = new GetBlogQuery() { Id = id };

        var response = await Mediator.Send(query);

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