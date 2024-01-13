using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogHub.Data.Blogs.Create.Helpers;
using BlogHub.Data.Blogs.Update.Helpers;
using BlogHub.Data.Comments.Create;
using BlogHub.Data.Tags.Link.Helpers;
using BlogHub.Data.Blogs.List.Helpers;
using BlogHub.Data.Blogs.List.User;
using BlogHub.Data.Pipeline.Helpers;
using BlogHub.Data.Pipeline.Steps.Blog.List.Add.Authors;
using BlogHub.Data.Pipeline.Steps.Blog.List.Add.Tags;
using BlogHub.Data.Pipeline.Steps.Blog.List.Search;
using BlogHub.Data.Pipeline.Steps.Blog.List.Sort;
using BlogHub.Data.Blogs.Create;
using BlogHub.Data.Blogs.Update;
using BlogHub.Data.Blogs.Delete;
using BlogHub.Data.Comments.Delete;
using BlogHub.Data.Tags.Link;
using BlogHub.Data.Tags.Unlink;

namespace BlogHub.Api.Controllers;

[Authorize]
public sealed class AuthorizeBlogController : BaseController
{
    public AuthorizeBlogController(IMediator mediator) : base(mediator) { }

    [HttpGet("my-blogs")]
    public async Task<ActionResult<BlogListVm>> GetAll([FromQuery] BlogListDto dto)
    {
        var query = new GetUserBlogsQuery() { Page = dto.List.Page, Size = dto.List.Size, UserId = UserId };

        var context = await Mediator.Send(query);

        var pipeline = new PipelineBuilder<BlogListVm>()
            .Add(new BlogListAddAuthorsStep(Mediator))
            .Add(new BlogListAddTagsStep(Mediator))
            .Add(new BlogListSearchStep(dto.Search, Mediator))
            .Add(new BlogListSortStep(dto.Sort, Mediator))
            .Build();

        var response = await pipeline.ExecuteAsync(context);

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

    [HttpPost("blog/{id}/comment/create")]
    public async Task<ActionResult<Guid>> CreateComment(Guid id, [FromBody] CreateCommentDto dto)
    {
        var command = new CreateCommentCommand
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
        var command = new DeleteCommentCommand
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
        var query = new LinkTagCommand
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
        var query = new UnlinkTagCommand
        {
            UserId = UserId,
            BlogId = id,
            TagId = tagId
        };

        var response = await Mediator.Send(query);

        return Ok(response);
    }
}