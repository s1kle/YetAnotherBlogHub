using BlogHub.Data.Pipeline.Helpers;
using AddAuthor = BlogHub.Data.Blogs.Get.Helpers.AddAuthor;

namespace BlogHub.Data.Pipeline.Steps.Blog.Add.Author;

public sealed class Step : IStep<BlogVm>
{
    private readonly IMediator _mediator;

    public Step(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BlogVm> ExecuteAsync(BlogVm context, Func<BlogVm, Task<BlogVm>> next)
    {
        var query = new AddAuthor.Query() { Blog = context };

        context = await _mediator.Send(query);

        return await next(context);
    }
}