using BlogHub.Data.Blogs.Queries.Get;
using BlogHub.Data.Comments.Queries.GetList.AddUser;
using BlogHub.Data.Comments.Queries.GetList.Blog;
using BlogHub.Data.Tags.Queries.GetList.Blog;

namespace BlogHub.Data.Pipeline.Blogs.Blog;

public sealed class AddCommentsStep : IPipelineStep<BlogVm>
{
    private readonly IMediator _mediator;

    public AddCommentsStep(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BlogVm> ExecuteAsync(BlogVm context, Func<BlogVm, Task<BlogVm>> next)
    {
        var query = new GetBlogCommentListQuery() { BlogId = context.Id };

        var comments = await _mediator.Send(query);

        var authorQuery = new CommentAddUserQueryQuery() { Comments = comments };

        comments = await _mediator.Send(authorQuery);

        context = context with { Comments = comments };

        return await next(context);
    }
}