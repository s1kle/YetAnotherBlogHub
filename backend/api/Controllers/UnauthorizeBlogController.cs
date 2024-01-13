using BlogHub.Data.Blogs.Get;
using BlogHub.Data.Blogs.Get.Helpers;
using BlogHub.Data.Blogs.List.All;
using BlogHub.Data.Blogs.List.Helpers;
using BlogHub.Data.Pipeline.Helpers;
using BlogHub.Data.Pipeline.Steps.Blog.Add.Author;
using BlogHub.Data.Pipeline.Steps.Blog.Add.Comments;
using BlogHub.Data.Pipeline.Steps.Blog.Add.Tags;
using BlogHub.Data.Pipeline.Steps.Blog.List.Add.Authors;
using BlogHub.Data.Pipeline.Steps.Blog.List.Add.Tags;
using BlogHub.Data.Pipeline.Steps.Blog.List.Search;
using BlogHub.Data.Pipeline.Steps.Blog.List.Sort;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

public sealed class UnauthorizeBlogController : BaseController
{
    public UnauthorizeBlogController(IMediator mediator) : base(mediator) { }

    [HttpGet("blogs")]
    public async Task<ActionResult<BlogListVm>> GetAll([FromQuery] BlogListDto dto)
    {
        var query = new GetAllBlogsQuery() { Page = dto.List.Page, Size = dto.List.Size };

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

    [HttpGet("blog/{id}")]
    public async Task<ActionResult<BlogVm>> Get(Guid id)
    {
        var query = new GetBlogQuery() { Id = id };

        var context = await Mediator.Send(query);

        var pipeline = new PipelineBuilder<BlogVm>()
            .Add(new BlogAddAuthorStep(Mediator))
            .Add(new BlogAddTagsStep(Mediator))
            .Add(new BlogAddCommentsStep(Mediator))
            .Build();

        var response = await pipeline.ExecuteAsync(context);

        return Ok(response);
    }
}