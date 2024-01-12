using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Comments.Queries.GetList.Blog;

namespace BlogHub.Data.Pipeline.Blogs.List;

public sealed class AddCommentsToListStep : IPipelineStep<BlogListVm>
{
    private readonly IMediator _mediator;

    public AddCommentsToListStep(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BlogListVm> ExecuteAsync(BlogListVm context, Func<BlogListVm, Task<BlogListVm>> next)
    {
        foreach (var blog in context.Blogs)
        {
            var query = new GetBlogCommentListQuery() { BlogId = blog.Id };

            blog.Comments = await _mediator.Send(query);
        }

        return await next(context);
    }
}