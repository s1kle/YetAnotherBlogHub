using BlogHub.Data.Blogs.Queries.Get;
using BlogHub.Data.Tags.Queries.GetList;

namespace BlogHub.Data.Pipeline.Blogs.Blog;

public sealed class AddTagsStep : IPipelineStep<BlogVm>
{
    private readonly IMediator _mediator;

    public AddTagsStep(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BlogVm> ExecuteAsync(BlogVm context, Func<BlogVm, Task<BlogVm>> next)
    {
        var query = new GetBlogTagListQuery() { BlogId = context.Id };

        context.Tags = await _mediator.Send(query);

        return await next(context);
    }
}