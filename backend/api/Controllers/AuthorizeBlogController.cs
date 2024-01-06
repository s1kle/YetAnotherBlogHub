using BlogHub.Data.Blogs.Commands.Create;
using BlogHub.Data.Blogs.Commands.Delete;
using BlogHub.Data.Blogs.Commands.Update;
using BlogHub.Data.Blogs.Queries.GetList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

[Authorize]
public sealed class AuthorizeBlogController : BaseBlogController
{
    public AuthorizeBlogController(IMediator mediator) : base (mediator) { }

    [HttpGet("my-blogs")]
    public async Task<ActionResult<BlogListVm>> GetAll([FromQuery] GetListDto dto)
    {
        var query = new GetUserBlogListQuery()
        {
            UserId = UserId,
            Page = dto.Page,
            Size = dto.Size,
            SortFilter = GetSortFilter(dto.SortProperty, dto.SortDirection),
            SearchFilter = GetSearchFilter(dto.SearchQuery, dto.SearchProperties)
        };

        var response = await Mediator.Send(query);

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

    [HttpPut("blog/update/id/{id}")]
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

    [HttpDelete("blog/delete/id/{id}")]
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
}