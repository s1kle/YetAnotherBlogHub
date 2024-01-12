using BlogHub.Data.Pipeline.Helpers;
using SortBy = BlogHub.Data.Blogs.List.Helpers.Sort;

namespace BlogHub.Data.Pipeline.Steps.Blog.List.Sort;

public sealed class Step : IStep<ListVm>
{
    private readonly IMediator _mediator;
    private readonly SortBy.Dto? _dto;

    public Step(SortBy.Dto? dto, IMediator mediator)
    {
        _mediator = mediator;
        _dto = dto;
    }

    public async Task<ListVm> ExecuteAsync(ListVm context, Func<ListVm, Task<ListVm>> next)
    {
        if (_dto is null || _dto.SortProperty is null)
            return await next(context);

        var query = new SortBy.Query() 
        {
            Blogs = context, 
            Property = _dto.SortProperty,
            Descending = _dto.SortDirection switch
            {
                "desc" => true,
                _ => false
            }
        };

        context = await _mediator.Send(query);

        return await next(context);
    }
}