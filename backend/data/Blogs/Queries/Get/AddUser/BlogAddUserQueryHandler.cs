namespace BlogHub.Data.Blogs.Queries.Get;

internal sealed class BlogAddUserQueryHandler : IRequestHandler<BlogAddUserQuery, BlogVm>
{
    private readonly IUserRepository _repository;

    public BlogAddUserQueryHandler(IUserRepository repository) =>
        _repository = repository;

    public async Task<BlogVm> Handle(BlogAddUserQuery request, CancellationToken cancellationToken)
    {
        var blog = request.Blog;

        var user = await _repository.GetAsync(request.Blog.UserId, cancellationToken);

        return blog with { Author = user?.Name };
    }
}