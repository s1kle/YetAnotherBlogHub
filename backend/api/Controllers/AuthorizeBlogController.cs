using BlogHub.Data.Blogs.Commands.Create;
using BlogHub.Data.Blogs.Commands.Delete;
using BlogHub.Data.Blogs.Commands.Update;
using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Blogs.Queries.ListSearch;
using BlogHub.Data.Blogs.Queries.ListSort;
using BlogHub.Data.Tags.Commands.Link;
using BlogHub.Data.Tags.Commands.Unlink;
using BlogHub.Data.Tags.Queries.GetList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

[Authorize]
public sealed class AuthorizeBlogController : BaseController
{
    public AuthorizeBlogController(IMediator mediator) : base (mediator) { }

    [HttpGet("my-blogs")]
    public async Task<ActionResult<BlogListVm>> GetAll([FromQuery] GetListDto dto,
        [FromQuery] ListSortDto sortDto, [FromQuery] ListSearchDto searchDto)
    {
        var query = new GetUserBlogListQuery()
        {
            UserId = UserId,
            Page = dto.Page,
            Size = dto.Size,
        };

        var response = await Mediator.Send(query);

        response = await ApplyFilters(response, sortDto, searchDto);

        return Ok(response);
    }

    [HttpPost("blog/create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateBlogDto dto)
    {
        var command = new CreateBlogCommand 
        { 
            Title = dto.Title,
            Details = dto.Details,
            UserId = UserId 
        };

        var blogId = await Mediator.Send(command);

        return Ok(blogId);
    }

    [HttpPut("blog/{id}/update")]
    public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] UpdateBlogDto dto)
    {
        var command = new UpdateBlogCommand 
        { 
            Id = id, 
            UserId = UserId,
            Title = dto.Title,
            Details = dto.Details
        };

        var blogId = await Mediator.Send(command);

        return Ok(blogId);
    }

    [HttpDelete("blog/{id}/delete")]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        var command = new DeleteBlogCommand
        {
            Id = id,
            UserId = UserId
        };

        var blogId = await Mediator.Send(command);

        return Ok(blogId);
    }

    [HttpPost("blog/{id}/tag/link")]
    public async Task<ActionResult<TagListVm>> LinkTag(Guid id, [FromBody] LinkTagDto dto)
    {
        var query = new LinkTagCommand()
        {
            UserId = UserId,
            BlogId = id,
            TagId = dto.TagId
        };

        var response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpDelete("blog/{id}/tag/{tagId}/unlink")]
    public async Task<ActionResult<TagListVm>> UnlinkTag(Guid id, Guid tagId)
    {
        var query = new UnlinkTagCommand()
        {
            UserId = UserId,
            BlogId = id,
            TagId = tagId
        };

        var response = await Mediator.Send(query);

        return Ok(response);
    }
}