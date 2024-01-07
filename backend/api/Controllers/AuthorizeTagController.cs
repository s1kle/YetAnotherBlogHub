using BlogHub.Data.Tags.Commands.Create;
using BlogHub.Data.Tags.Commands.Delete;
using BlogHub.Data.Tags.Queries.GetList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

public class AuthorizeTagController : BaseController
{
    public AuthorizeTagController(IMediator mediator) : base (mediator) { }

    [HttpGet("my-tags")]
    public async Task<ActionResult<TagListVm>> GetAll()
    {
        var query = new GetUserTagListQuery()
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

        var blogId = await Mediator.Send(command);

        return Ok(blogId);
    }

    [HttpDelete("tag/{id}/delete")]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        var command = new DeleteTagCommand
        {
            Id = id,
            UserId = UserId
        };

        var blogId = await Mediator.Send(command);

        return Ok(blogId);
    }
}