using BlogHub.Data.Pipeline.Helpers;
using GetComments = BlogHub.Data.Comments.List.Blog;
using AddAuthor = BlogHub.Data.Comments.List.Helpers.AddAuthor;

namespace BlogHub.Data.Pipeline.Steps.Blog.Add.Comments;

public sealed class Step : IStep<BlogVm>
{
    private readonly IMediator _mediator;

    public Step(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BlogVm> ExecuteAsync(BlogVm context, Func<BlogVm, Task<BlogVm>> next)
    {
        var query = new GetComments.Query() { BlogId = context.Id };

        var comments = await _mediator.Send(query);

        var authorQuery = new AddAuthor.Query() { Comments = comments };

        comments = await _mediator.Send(authorQuery);

        context = context with { Comments = comments };

        return await next(context);
    }
}