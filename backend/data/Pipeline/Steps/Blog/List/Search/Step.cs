using BlogHub.Data.Pipeline.Helpers;
using SearchBy = BlogHub.Data.Blogs.List.Helpers.Search;

namespace BlogHub.Data.Pipeline.Steps.Blog.List.Search;

public sealed class Step : IStep<ListVm>
{
    private readonly IMediator _mediator;
    private readonly SearchBy.Dto? _dto;

    public Step(SearchBy.Dto? dto, IMediator mediator)
    {
        _mediator = mediator;
        _dto = dto;
    }

    public async Task<ListVm> ExecuteAsync(ListVm context, Func<ListVm, Task<ListVm>> next)
    {
        if (_dto is null || _dto.SearchQuery is null)
            return await next(context);

        var query = new SearchBy.Query() 
        {
            Blogs = context, 
            Filter = _dto.SearchQuery, 
            Properties = _dto.SearchProperties?.Split(' ') ?? Array.Empty<string>()
        };

        context = await _mediator.Send(query);

        return await next(context);
    }
}