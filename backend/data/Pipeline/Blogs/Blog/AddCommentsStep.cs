using BlogHub.Data.Blogs.Queries.Get;
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

        context.Comments = await _mediator.Send(query);

        return await next(context);
    }
}