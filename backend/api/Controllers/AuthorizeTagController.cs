using BlogHub.Data.Tags.Get.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BlogHub.Data.Tags.Create.Helpers;
using BlogHub.Data.Tags.List.User;
using BlogHub.Data.Tags.Create;
using BlogHub.Data.Tags.Delete;

namespace BlogHub.Api.Controllers;

public class AuthorizeTagController : BaseController
{
    public AuthorizeTagController(IMediator mediator) : base(mediator) { }

    [HttpGet("my-tags")]
    public async Task<ActionResult<IReadOnlyList<TagVm>>> GetAll()
    {
        var query = new GetUserTagsQuery()
        {
            UserId = UserId
        };

        var response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpPost("tag/create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateTagDto dto)
    {
        var command = new CreateTagCommand
        {
            UserId = UserId,
            Name = dto.Name
        };

        var ArticleId = await Mediator.Send(command);

        return Ok(ArticleId);
    }

    [HttpDelete("tag/{id}/delete")]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        var command = new DeleteTagCommand
        {
            Id = id,
            UserId = UserId
        };

        var ArticleId = await Mediator.Send(command);

        return Ok(ArticleId);
    }
}