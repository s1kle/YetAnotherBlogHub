using BlogHub.Data.Comments.List.Blog;
using BlogHub.Data.Comments.List.Helpers.AddAuthor;
using BlogHub.Data.Pipeline.Helpers;

namespace BlogHub.Data.Pipeline.Steps.Blog.Add.Comments;

public sealed class BlogAddCommentsStep : IPipelineStep<BlogVm>
{
    private readonly IMediator _mediator;

    public BlogAddCommentsStep(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BlogVm> ExecuteAsync(BlogVm context, Func<BlogVm, Task<BlogVm>> next)
    {
        var query = new GetBlogCommentsQuery() { BlogId = context.Id };

        var comments = await _mediator.Send(query);

        var authorQuery = new CommentListAddAuthorsQuery() { Comments = comments };

        comments = await _mediator.Send(authorQuery);

        context = context with { Comments = comments };

        return await next(context);
    }
}