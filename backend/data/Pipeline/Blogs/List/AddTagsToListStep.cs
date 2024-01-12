using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Tags.Queries.GetList.Blog;

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

        var result = context.Blogs.ToList();

        for (var i = 0; i < result.Count; i++)
        {
            var blog = result[i];
            var query = new GetBlogTagListQuery() { BlogId = blog.Id };
            var tags = await _mediator.Send(query);
            result[i] = blog with { Tags = tags };
        }

        context = context with { Blogs = result };

        return await next(context);
    }
}