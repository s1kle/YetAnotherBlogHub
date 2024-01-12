using BlogHub.Data.Blogs.Commands.Create;
using BlogHub.Data.Blogs.Commands.Delete;
using BlogHub.Data.Blogs.Commands.Update;
using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Blogs.Queries.GetList.User;
using BlogHub.Data.Blogs.Queries.ListSearch;
using BlogHub.Data.Blogs.Queries.ListSort;
using BlogHub.Data.Pipeline;
using BlogHub.Data.Pipeline.Blogs.List;
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
        [FromQuery] ListSortDto? sortDto = null, [FromQuery] ListSearchDto? searchDto = null)
    {
        var query = new GetUserBlogListQuery() { Page = dto.Page, Size = dto.Size, UserId = UserId };

        var context = await Mediator.Send(query);

        var pipeline = new PipelineBuilder<BlogListVm>()
            .Add(new AddAuthorsStep(Mediator))
            .Add(new AddTagsToListStep(Mediator))
            .Add(new SearchStep(searchDto, Mediator))
            .Add(new SortStep(sortDto, Mediator))
            .Build();

        var response = await pipeline(context);

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
    public async Task<ActionResult<Guid>> LinkTag(Guid id, [FromBody] LinkTagDto dto)
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
    public async Task<ActionResult<Guid>> UnlinkTag(Guid id, Guid tagId)
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