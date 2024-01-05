using BlogHub.Data.Blogs.Queries.Get;
using BlogHub.Data.Blogs.Queries.GetList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

public sealed class UnauthorizeBlogController : BaseBlogController
{
    public UnauthorizeBlogController(IMediator mediator) : base (mediator) { }

    [HttpGet("blogs")]
    public async Task<ActionResult<BlogListVm>> GetAll([FromQuery] GetListDto dto)
    {
        var query = ParseGetListDto(dto);

        var response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet("blog/get/id/{id}")]
    public async Task<ActionResult<BlogVm>> Get(Guid id)
    {
        var query = new GetBlogQuery() { Id = id };

        var response = await Mediator.Send(query);

        return Ok(response);
    }
}