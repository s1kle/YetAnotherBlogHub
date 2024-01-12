using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Blogs.Queries.GetList.AddUser;

namespace BlogHub.Data.Pipeline.Blogs.List;

public sealed class AddAuthorsStep : IPipelineStep<BlogListVm>
{
    private readonly IMediator _mediator;

    public AddAuthorsStep(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BlogListVm> ExecuteAsync(BlogListVm context, Func<BlogListVm, Task<BlogListVm>> next)
    {
        var query = new ListAddUserQuery() { Blogs = context };

        context = await _mediator.Send(query);

        return await next(context);
    }
}