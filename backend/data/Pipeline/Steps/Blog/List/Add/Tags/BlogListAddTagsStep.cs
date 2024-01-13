using BlogHub.Data.Pipeline.Helpers;
using BlogHub.Data.Tags.List.Blog;

namespace BlogHub.Data.Pipeline.Steps.Blog.List.Add.Tags;

public sealed class BlogListAddTagsStep : IPipelineStep<BlogListVm>
{
    private readonly IMediator _mediator;

    public BlogListAddTagsStep(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BlogListVm> ExecuteAsync(BlogListVm context, Func<BlogListVm, Task<BlogListVm>> next)
    {

        var result = context.Blogs.ToList();

        for (var i = 0; i < result.Count; i++)
        {
            var blog = result[i];

            var query = new GetBlogTagsQuery() { BlogId = blog.Id };

            var tags = await _mediator.Send(query);

            result[i] = blog with { Tags = tags };
        }

        context = context with { Blogs = result };

        return await next(context);
    }
}