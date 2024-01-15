using BlogHub.Data.Articles.List.Helpers.Search;
using BlogHub.Data.Pipeline.Helpers;

namespace BlogHub.Data.Pipeline.Steps.Article.List.Search;

public sealed class ArticleListSearchStep : IPipelineStep<ArticleListVm>
{
    private readonly IMediator _mediator;
    private readonly SearchDto? _dto;

    public ArticleListSearchStep(SearchDto? dto, IMediator mediator)
    {
        _mediator = mediator;
        _dto = dto;
    }

    public async Task<ArticleListVm> ExecuteAsync(ArticleListVm context, Func<ArticleListVm, Task<ArticleListVm>> next)
    {
        if (_dto is null || _dto.Query is null)
            return await next(context);

        var query = new ArticleListSearchQuery()
        {
            Articles = context,
            Query = _dto.Query,
            Properties = _dto.Properties?.Split(' ') ?? Array.Empty<string>()
        };

        context = await _mediator.Send(query);

        return await next(context);
    }
}