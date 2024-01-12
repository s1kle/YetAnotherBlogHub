using BlogHub.Data.Pipeline.Helpers;
using AddAuthor = BlogHub.Data.Blogs.List.Helpers.AddAuthors;

namespace BlogHub.Data.Pipeline.Steps.Blog.List.Add.Authors;

public sealed class Step : IStep<ListVm>
{
    private readonly IMediator _mediator;

    public Step(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ListVm> ExecuteAsync(ListVm context, Func<ListVm, Task<ListVm>> next)
    {
        var query = new AddAuthor.Query() { Blogs = context };

        context = await _mediator.Send(query);

        return await next(context);
    }
}