using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Blogs.Queries.ListSearch;

namespace BlogHub.Data.Pipeline.Blogs.List;

public sealed class SearchStep : IPipelineStep<BlogListVm>
{
    private readonly IMediator _mediator;
    private readonly ListSearchDto? _dto;

    public SearchStep(ListSearchDto? dto, IMediator mediator)
    {
        _mediator = mediator;
        _dto = dto;
    }

    public async Task<BlogListVm> ExecuteAsync(BlogListVm context, Func<BlogListVm, Task<BlogListVm>> next)
    {
        if (_dto is null || _dto.SearchQuery is null)
            return await next(context);

        var query = new ListSearchQuery() 
        {
            Blogs = context, 
            Query = _dto.SearchQuery, 
            Properties = _dto.SearchProperties?.Split(' ') ?? Array.Empty<string>()
        };

        context = await _mediator.Send(query);

        return await next(context);
    }
}