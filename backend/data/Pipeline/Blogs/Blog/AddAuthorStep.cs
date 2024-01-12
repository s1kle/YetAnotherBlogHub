using BlogHub.Data.Blogs.Queries.Get;

namespace BlogHub.Data.Pipeline.Blogs.Blog;

public sealed class AddAuthorStep : IPipelineStep<BlogVm>
{
    private readonly IMediator _mediator;

    public AddAuthorStep(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BlogVm> ExecuteAsync(BlogVm context, Func<BlogVm, Task<BlogVm>> next)
    {
        var query = new BlogAddUserQuery() { Blog = context };

        context = await _mediator.Send(query);

        return await next(context);
    }
}