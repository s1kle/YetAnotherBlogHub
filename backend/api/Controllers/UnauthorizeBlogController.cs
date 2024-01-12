using BlogHub.Data.Blogs.Get.Helpers;
using BlogHub.Data.Blogs.List.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AllBlogs = BlogHub.Data.Blogs.List.All;
using BlogDetails = BlogHub.Data.Blogs.Get;
using Pipeline = BlogHub.Data.Pipeline.Helpers;
using Steps = BlogHub.Data.Pipeline.Steps.Blog;

namespace BlogHub.Api.Controllers;

public sealed class UnauthorizeBlogController : BaseController
{
    public UnauthorizeBlogController(IMediator mediator) : base (mediator) { }

    [HttpGet("blogs")]
    public async Task<ActionResult<ListVm>> GetAll([FromQuery] BlogListDto dto)
    {
        var query = new AllBlogs.Query() { Page = dto.List.Page, Size = dto.List.Size };

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

    [HttpGet("blog/{id}")]
    public async Task<ActionResult<BlogVm>> Get(Guid id)
    {
        var query = new BlogDetails.Query() { Id = id };

        var context = await Mediator.Send(query);

        var pipeline = new Pipeline.Builder<BlogVm>()
            .Add(new Steps.Add.Author.Step(Mediator))
            .Add(new Steps.Add.Tags.Step(Mediator))
            .Add(new Steps.Add.Comments.Step(Mediator))
            .Build();

        var response = await pipeline.ExecuteAsync(context);

        return Ok(response);
    }
}