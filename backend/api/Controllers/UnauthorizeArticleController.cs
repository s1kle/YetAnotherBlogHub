using BlogHub.Data.Articles.Get;
using BlogHub.Data.Articles.Get.Helpers;
using BlogHub.Data.Articles.List.All;
using BlogHub.Data.Articles.List.Helpers;
using BlogHub.Data.Pipeline.Helpers;
using BlogHub.Data.Pipeline.Steps.Article.List.Search;
using BlogHub.Data.Pipeline.Steps.Article.List.Sort;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Api.Controllers;

public sealed class UnauthorizeArticleController : BaseController
{
    public UnauthorizeArticleController(IMediator mediator) : base(mediator) { }

    [HttpGet("Articles")]
    public async Task<ActionResult<ArticleListVm>> GetAll([FromQuery] ArticleListDto dto)
    {
        var query = new GetAllArticlesQuery() { Page = dto.List.Page, Size = dto.List.Size };

        var context = await Mediator.Send(query);

        var pipeline = new PipelineBuilder<ArticleListVm>()
            .Add(new ArticleListSearchStep(dto.Search, Mediator))
            .Add(new ArticleListSortStep(dto.Sort, Mediator))
            .Build();

        var response = await pipeline.ExecuteAsync(context);

        return Ok(response);
    }

    [HttpGet("Article/{id}")]
    public async Task<ActionResult<ArticleVm>> Get(Guid id)
    {
        var query = new GetArticleQuery() { Id = id };

        var response = await Mediator.Send(query);

        return Ok(response);
    }
}