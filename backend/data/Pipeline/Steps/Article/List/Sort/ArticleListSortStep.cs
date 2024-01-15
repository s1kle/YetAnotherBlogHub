using BlogHub.Data.Articles.List.Helpers.Sort;
using BlogHub.Data.Pipeline.Helpers;

namespace BlogHub.Data.Pipeline.Steps.Article.List.Sort;

public sealed class ArticleListSortStep : IPipelineStep<ArticleListVm>
{
    private readonly IMediator _mediator;
    private readonly SortDto? _dto;

    public ArticleListSortStep(SortDto? dto, IMediator mediator)
    {
        _mediator = mediator;
        _dto = dto;
    }

    public async Task<ArticleListVm> ExecuteAsync(ArticleListVm context, Func<ArticleListVm, Task<ArticleListVm>> next)
    {
        if (_dto is null || _dto.Property is null)
            return await next(context);

        var query = new ArticleListSortQuery()
        {
            Articles = context,
            Property = _dto.Property,
            Descending = _dto.Direction switch
            {
                "desc" => true,
                _ => false
            }
        };

        context = await _mediator.Send(query);

        return await next(context);
    }
}