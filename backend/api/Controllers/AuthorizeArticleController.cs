using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogHub.Data.Articles.Create.Helpers;
using BlogHub.Data.Articles.Update.Helpers;
using BlogHub.Data.Comments.Create;
using BlogHub.Data.Tags.Link.Helpers;
using BlogHub.Data.Articles.List.Helpers;
using BlogHub.Data.Articles.List.User;
using BlogHub.Data.Pipeline.Helpers;
using BlogHub.Data.Pipeline.Steps.Article.List.Search;
using BlogHub.Data.Pipeline.Steps.Article.List.Sort;
using BlogHub.Data.Articles.Create;
using BlogHub.Data.Articles.Update;
using BlogHub.Data.Articles.Delete;
using BlogHub.Data.Comments.Delete;
using BlogHub.Data.Tags.Link;
using BlogHub.Data.Tags.Unlink;

namespace BlogHub.Api.Controllers;

[Authorize]
public sealed class AuthorizeArticleController : BaseController
{
    public AuthorizeArticleController(IMediator mediator) : base(mediator) { }

    [HttpGet("my-Articles")]
    public async Task<ActionResult<ArticleListVm>> GetAll([FromQuery] ArticleListDto dto)
    {
        var query = new GetUserArticlesQuery() { Page = dto.List.Page, Size = dto.List.Size, UserId = UserId };

        var context = await Mediator.Send(query);

        var pipeline = new PipelineBuilder<ArticleListVm>()
            .Add(new ArticleListSearchStep(dto.Search, Mediator))
            .Add(new ArticleListSortStep(dto.Sort, Mediator))
            .Build();

        var response = await pipeline.ExecuteAsync(context);

        return Ok(response);
    }

    [HttpPost("Article/create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateArticleDto dto)
    {
        var command = new CreateArticleCommand
        {
            Title = dto.Title,
            Details = dto.Details,
            UserId = UserId
        };

        var ArticleId = await Mediator.Send(command);

        return Ok(ArticleId);
    }

    [HttpPut("Article/{id}/update")]
    public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] UpdateArticleDto dto)
    {
        var command = new UpdateArticleCommand
        {
            Id = id,
            UserId = UserId,
            Title = dto.Title,
            Details = dto.Details
        };

        var ArticleId = await Mediator.Send(command);

        return Ok(ArticleId);
    }

    [HttpDelete("Article/{id}/delete")]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        var command = new DeleteArticleCommand
        {
            Id = id,
            UserId = UserId
        };

        var ArticleId = await Mediator.Send(command);

        return Ok(ArticleId);
    }

    [HttpPost("Article/{id}/comment/create")]
    public async Task<ActionResult<Guid>> CreateComment(Guid id, [FromBody] CreateCommentDto dto)
    {
        var command = new CreateCommentCommand
        {
            UserId = UserId,
            ArticleId = id,
            Content = dto.Content
        };

        var ArticleId = await Mediator.Send(command);

        return Ok(ArticleId);
    }

    [HttpDelete("Article/{id}/comment/{commentId}/delete")]
    public async Task<ActionResult<Guid>> DeleteComment(Guid id, Guid commentId)
    {
        var command = new DeleteCommentCommand
        {
            UserId = UserId,
            Id = commentId
        };

        var ArticleId = await Mediator.Send(command);

        return Ok(ArticleId);
    }

    [HttpPost("Article/{id}/tag/link")]
    public async Task<ActionResult<Guid>> LinkTag(Guid id, [FromBody] LinkTagDto dto)
    {
        var query = new LinkTagCommand
        {
            UserId = UserId,
            ArticleId = id,
            TagId = dto.TagId
        };

        var response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpDelete("Article/{id}/tag/{tagId}/unlink")]
    public async Task<ActionResult<Guid>> UnlinkTag(Guid id, Guid tagId)
    {
        var query = new UnlinkTagCommand
        {
            UserId = UserId,
            ArticleId = id,
            TagId = tagId
        };

        var response = await Mediator.Send(query);

        return Ok(response);
    }
}