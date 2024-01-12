using BlogHub.Data.Tags.Get.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TagList = BlogHub.Data.Tags.List.User;
using CreateTag = BlogHub.Data.Tags.Create;
using DeleteTag = BlogHub.Data.Tags.Delete;
using BlogHub.Data.Tags.Create.Helpers;

namespace BlogHub.Api.Controllers;

public class AuthorizeTagController : BaseController
{
    public AuthorizeTagController(IMediator mediator) : base (mediator) { }

    [HttpGet("my-tags")]
    public async Task<ActionResult<IReadOnlyList<TagVm>>> GetAll()
    {
        var query = new TagList.Query()
        {
            UserId = UserId
        };

        var response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpPost("tag/create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateTagDto dto)
    {
        var command = new CreateTag.Command 
        { 
            UserId = UserId,
            Name = dto.Name
        };

        var blogId = await Mediator.Send(command);

        return Ok(blogId);
    }

    [HttpDelete("tag/{id}/delete")]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        var command = new DeleteTag.Command
        {
            Id = id,
            UserId = UserId
        };

        var blogId = await Mediator.Send(command);

        return Ok(blogId);
    }
}