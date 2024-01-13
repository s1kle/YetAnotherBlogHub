using BlogHub.Data.Blogs.Get.Helpers.AddAuthor;
using BlogHub.Data.Pipeline.Helpers;

namespace BlogHub.Data.Pipeline.Steps.Blog.Add.Author;

public sealed class BlogAddAuthorStep : IPipelineStep<BlogVm>
{
    private readonly IMediator _mediator;

    public BlogAddAuthorStep(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BlogVm> ExecuteAsync(BlogVm context, Func<BlogVm, Task<BlogVm>> next)
    {
        var query = new BlogAddAuthorQuery() { Blog = context };

        context = await _mediator.Send(query);

        return await next(context);
    }
}