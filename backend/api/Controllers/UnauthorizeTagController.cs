using BlogHub.Data.Tags.Queries.Get;
using BlogHub.Data.Tags.Queries.GetList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

public sealed class UnauthorizeTagController : BaseController
{
    public UnauthorizeTagController(IMediator mediator) : base (mediator) { }

    [HttpGet("tags")]
    public async Task<ActionResult<IReadOnlyList<TagVm>>> GetAll()
    {
        var query = new GetTagListQuery();

        var response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet("tag/{id}")]
    public async Task<ActionResult<TagVm>> Get(Guid id)
    {
        var query = new GetTagQuery() { Id = id };

        var response = await Mediator.Send(query);

        return Ok(response);
    }
}