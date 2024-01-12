using BlogHub.Data.Tags.Queries.Get;
using BlogHub.Data.Tags.Queries.GetList.All;
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
}