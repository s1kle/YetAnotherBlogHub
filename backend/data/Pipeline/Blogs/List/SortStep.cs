using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Blogs.Queries.ListSort;

namespace BlogHub.Data.Pipeline.Blogs.List;

public sealed class SortStep : IPipelineStep<BlogListVm>
{
    private readonly IMediator _mediator;
    private readonly ListSortDto? _dto;

    public SortStep(ListSortDto? dto, IMediator mediator)
    {
        _mediator = mediator;
        _dto = dto;
    }

    public async Task<BlogListVm> ExecuteAsync(BlogListVm context, Func<BlogListVm, Task<BlogListVm>> next)
    {
        if (_dto is null || _dto.SortProperty is null)
            return await next(context);

        var query = new ListSortQuery() 
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