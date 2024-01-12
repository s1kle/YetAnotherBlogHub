using BlogHub.Data.Tags.Get.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TagList = BlogHub.Data.Tags.List.All;

namespace BlogHub.Api.Controllers;

public sealed class UnauthorizeTagController : BaseController
{
    public UnauthorizeTagController(IMediator mediator) : base (mediator) { }

    [HttpGet("tags")]
    public async Task<ActionResult<IReadOnlyList<TagVm>>> GetAll()
    {
        var query = new TagList.Query();

        var response = await Mediator.Send(query);

        return Ok(response);
    }
}