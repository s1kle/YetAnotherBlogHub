using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Tags.Queries.GetList;

namespace BlogHub.Data.Pipeline.Blogs.List;

public sealed class AddTagsToListStep : IPipelineStep<BlogListVm>
{
    private readonly IMediator _mediator;

    public AddTagsToListStep(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BlogListVm> ExecuteAsync(BlogListVm context, Func<BlogListVm, Task<BlogListVm>> next)
    {
        foreach (var blog in context.Blogs)
        {
            var query = new GetBlogTagListQuery() { BlogId = blog.Id };

            blog.Tags = await _mediator.Send(query);
        }

        return await next(context);
    }
}