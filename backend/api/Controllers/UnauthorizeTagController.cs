using BlogHub.Data.Blogs.Queries.Get;
using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Tags.Queries.Get;
using BlogHub.Data.Tags.Queries.GetList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

public sealed class UnauthorizeTagController : BlogHubController
{
    public UnauthorizeTagController(IMediator mediator) : base (mediator) { }

    [HttpGet("tags")]
    public async Task<ActionResult<BlogListVm>> GetAll()
    {
        var query = new GetTagListQuery();

        var response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet("tag/get/id/{id}")]
    public async Task<ActionResult<BlogVm>> Get(Guid id)
    {
        var query = new GetTagQuery() { Id = id };

        var response = await Mediator.Send(query);

        return Ok(response);
    }
}