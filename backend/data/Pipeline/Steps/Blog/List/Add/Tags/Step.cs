using BlogHub.Data.Pipeline.Helpers;
using GetTags = BlogHub.Data.Tags.List.Blog;

namespace BlogHub.Data.Pipeline.Steps.Blog.List.Add.Tags;

public sealed class Step : IStep<ListVm>
{
    private readonly IMediator _mediator;

    public Step(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ListVm> ExecuteAsync(ListVm context, Func<ListVm, Task<ListVm>> next)
    {

        var result = context.Blogs.ToList();

        for (var i = 0; i < result.Count; i++)
        {
            var blog = result[i];

            var query = new GetTags.Query() { BlogId = blog.Id };

            var tags = await _mediator.Send(query);
            
            result[i] = blog with { Tags = tags };
        }

        context = context with { Blogs = result };

        return await next(context);
    }
}