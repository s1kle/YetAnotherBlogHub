using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserBlogs = BlogHub.Data.Blogs.List.User;
using Pipeline = BlogHub.Data.Pipeline.Helpers;
using Steps = BlogHub.Data.Pipeline.Steps.Blog;
using BlogHub.Data.Blogs.List.Helpers;
using CreateBlog = BlogHub.Data.Blogs.Create;
using UpdateBlog = BlogHub.Data.Blogs.Update;
using DeleteBlog = BlogHub.Data.Blogs.Delete;
using CreateComment = BlogHub.Data.Comments.Create;
using DeleteComment = BlogHub.Data.Comments.Delete;
using LinkTag = BlogHub.Data.Tags.Link;
using UnlinkTag = BlogHub.Data.Tags.Unlink;
using BlogHub.Data.Blogs.Create.Helpers;
using BlogHub.Data.Blogs.Update.Helpers;
using BlogHub.Data.Comments.Create;
using BlogHub.Data.Tags.Link.Helpers;

namespace BlogHub.Api.Controllers;

[Authorize]
public sealed class AuthorizeBlogController : BaseController
{
    public AuthorizeBlogController(IMediator mediator) : base (mediator) { }

    [HttpGet("my-blogs")]
    public async Task<ActionResult<ListVm>> GetAll([FromQuery] BlogListDto dto)
    {
        var query = new UserBlogs.Query() { Page = dto.List.Page, Size = dto.List.Size, UserId = UserId };

        var context = await Mediator.Send(query);

        var pipeline = new Pipeline.Builder<ListVm>()
            .Add(new Steps.List.Add.Authors.Step(Mediator))
            .Add(new Steps.List.Add.Tags.Step(Mediator))
            .Add(new Steps.List.Search.Step(dto.Search, Mediator))
            .Add(new Steps.List.Sort.Step(dto.Sort, Mediator))
            .Build();

        var response = await pipeline.ExecuteAsync(context);

        return Ok(response);
    }

    [HttpPost("blog/create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateBlogDto dto)
    {
        var command = new CreateBlog.Command 
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
        var command = new UpdateBlog.Command 
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
        var command = new DeleteBlog.Command
        {
            Id = id,
            UserId = UserId
        };

        var blogId = await Mediator.Send(command);

        return Ok(blogId);
    }

    [HttpPost("blog/{id}/comment/create")]
    public async Task<ActionResult<Guid>> CreateComment(Guid id, [FromBody] CreateCommentDto dto)
    {
        var command = new CreateComment.Command 
        { 
            UserId = UserId,
            BlogId = id,
            Content = dto.Content
        };

        var blogId = await Mediator.Send(command);

        return Ok(blogId);
    }

    [HttpDelete("blog/{id}/comment/{commentId}/delete")]
    public async Task<ActionResult<Guid>> DeleteComment(Guid id, Guid commentId)
    {
        var command = new DeleteComment.Command 
        { 
            UserId = UserId,
            Id = commentId
        };

        var blogId = await Mediator.Send(command);

        return Ok(blogId);
    }

    [HttpPost("blog/{id}/tag/link")]
    public async Task<ActionResult<Guid>> LinkTag(Guid id, [FromBody] LinkTagDto dto)
    {
        var query = new LinkTag.Command()
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
        var query = new UnlinkTag.Command()
        {
            UserId = UserId,
            BlogId = id,
            TagId = tagId
        };

        var response = await Mediator.Send(query);

        return Ok(response);
    }
}