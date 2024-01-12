namespace BlogHub.Data.Blogs.Get.Helpers.AddAuthor;

internal sealed class Handler : IRequestHandler<Query, BlogVm>
{
    private readonly UsersContext.Repository _repository;

    public Handler(UsersContext.Repository repository) =>
        _repository = repository;

    public async Task<BlogVm> Handle(Query request, CancellationToken cancellationToken)
    {
        var blog = request.Blog;

        var user = await _repository.GetAsync(request.Blog.UserId, cancellationToken);

        return blog with { Author = user?.Name };
    }
}