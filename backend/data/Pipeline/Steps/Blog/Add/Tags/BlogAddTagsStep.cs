using BlogHub.Data.Pipeline.Helpers;
using BlogHub.Data.Tags.List.Blog;

namespace BlogHub.Data.Pipeline.Steps.Blog.Add.Tags;

public sealed class BlogAddTagsStep : IPipelineStep<BlogVm>
{
    private readonly IMediator _mediator;

    public BlogAddTagsStep(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BlogVm> ExecuteAsync(BlogVm context, Func<BlogVm, Task<BlogVm>> next)
    {
        var query = new GetBlogTagsQuery() { BlogId = context.Id };

        context = context with { Tags = await _mediator.Send(query) };

        return await next(context);
    }
}