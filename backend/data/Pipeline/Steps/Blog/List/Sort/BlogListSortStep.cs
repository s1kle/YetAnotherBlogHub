using BlogHub.Data.Blogs.List.Helpers.Sort;
using BlogHub.Data.Pipeline.Helpers;

namespace BlogHub.Data.Pipeline.Steps.Blog.List.Sort;

public sealed class BlogListSortStep : IPipelineStep<BlogListVm>
{
    private readonly IMediator _mediator;
    private readonly SortDto? _dto;

    public BlogListSortStep(SortDto? dto, IMediator mediator)
    {
        _mediator = mediator;
        _dto = dto;
    }

    public async Task<BlogListVm> ExecuteAsync(BlogListVm context, Func<BlogListVm, Task<BlogListVm>> next)
    {
        if (_dto is null || _dto.SortProperty is null)
            return await next(context);

        var query = new BlogListSortQuery()
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