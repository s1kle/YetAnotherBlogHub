using BlogHub.Data.Tags.Commands.Create;
using BlogHub.Data.Tags.Commands.Delete;
using BlogHub.Data.Tags.Queries.GetList;
using BlogHub.Data.Tags.Queries.GetList.ByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

public class AuthorizeTagController : BlogHubController
{
    public AuthorizeTagController(IMediator mediator) : base (mediator) { }

    [HttpGet("my-tags")]
    public async Task<ActionResult<TagListVm>> GetAll()
    {
        var query = new GetTagListByUserIdQuery()
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

    [HttpDelete("tag/delete/id/{id}")]
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