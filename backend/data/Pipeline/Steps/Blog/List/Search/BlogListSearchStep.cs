using BlogHub.Data.Blogs.List.Helpers.Search;
using BlogHub.Data.Pipeline.Helpers;

namespace BlogHub.Data.Pipeline.Steps.Blog.List.Search;

public sealed class BlogListSearchStep : IPipelineStep<BlogListVm>
{
    private readonly IMediator _mediator;
    private readonly SearchDto? _dto;

    public BlogListSearchStep(SearchDto? dto, IMediator mediator)
    {
        _mediator = mediator;
        _dto = dto;
    }

    public async Task<BlogListVm> ExecuteAsync(BlogListVm context, Func<BlogListVm, Task<BlogListVm>> next)
    {
        if (_dto is null || _dto.SearchQuery is null)
            return await next(context);

        var query = new BlogListSearchQuery()
        {
            Blogs = context,
            Filter = _dto.SearchQuery,
            Properties = _dto.SearchProperties?.Split(' ') ?? Array.Empty<string>()
        };

        context = await _mediator.Send(query);

        return await next(context);
    }
}