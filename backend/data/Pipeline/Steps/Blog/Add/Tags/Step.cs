using BlogHub.Data.Pipeline.Helpers;
using GetTags = BlogHub.Data.Tags.List.Blog;

namespace BlogHub.Data.Pipeline.Steps.Blog.Add.Tags;

public sealed class Step : IStep<BlogVm>
{
    private readonly IMediator _mediator;

    public Step(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BlogVm> ExecuteAsync(BlogVm context, Func<BlogVm, Task<BlogVm>> next)
    {
        var query = new GetTags.Query() { BlogId = context.Id };

        context = context with { Tags = await _mediator.Send(query) };

        return await next(context);
    }
}