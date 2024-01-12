using BlogHub.Data.Blogs.Queries.Get;
using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Blogs.Queries.GetList.All;
using BlogHub.Data.Blogs.Queries.ListSearch;
using BlogHub.Data.Blogs.Queries.ListSort;
using BlogHub.Data.Pipeline;
using BlogHub.Data.Pipeline.Blogs.Blog;
using BlogHub.Data.Pipeline.Blogs.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

public sealed class UnauthorizeBlogController : BaseController
{
    public UnauthorizeBlogController(IMediator mediator) : base (mediator) { }

    [HttpGet("blogs")]
    public async Task<ActionResult<BlogListVm>> GetAll([FromQuery] GetListDto dto,
        [FromQuery] ListSortDto? sortDto = null, [FromQuery] ListSearchDto? searchDto = null)
    {
        var query = new GetBlogListQuery() { Page = dto.Page, Size = dto.Size };

        var context = await Mediator.Send(query);

        var pipeline = new PipelineBuilder<BlogListVm>()
            .Add(new AddAuthorsStep(Mediator))
            .Add(new AddTagsToListStep(Mediator))
            .Add(new AddCommentsToListStep(Mediator))
            .Add(new SearchStep(searchDto, Mediator))
            .Add(new SortStep(sortDto, Mediator))
            .Build();

        var response = await pipeline(context);

        return Ok(response);
    }

    [HttpGet("blog/{id}")]
    public async Task<ActionResult<BlogVm>> Get(Guid id)
    {
        var query = new GetBlogQuery() { Id = id };

        var context = await Mediator.Send(query);

        var pipeline = new PipelineBuilder<BlogVm>()
            .Add(new AddAuthorStep(Mediator))
            .Add(new AddTagsStep(Mediator))
            .Add(new AddCommentsStep(Mediator))
            .Build();

        var response = await pipeline(context);

        return Ok(response);
    }
}