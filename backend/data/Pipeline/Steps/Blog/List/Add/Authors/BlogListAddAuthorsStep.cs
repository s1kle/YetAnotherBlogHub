using BlogHub.Data.Blogs.List.Helpers.AddAuthors;
using BlogHub.Data.Pipeline.Helpers;

namespace BlogHub.Data.Pipeline.Steps.Blog.List.Add.Authors;

public sealed class BlogListAddAuthorsStep : IPipelineStep<BlogListVm>
{
    private readonly IMediator _mediator;

    public BlogListAddAuthorsStep(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BlogListVm> ExecuteAsync(BlogListVm context, Func<BlogListVm, Task<BlogListVm>> next)
    {
        var query = new BlogListAddAuthorsQuery() { Blogs = context };

        context = await _mediator.Send(query);

        return await next(context);
    }
}