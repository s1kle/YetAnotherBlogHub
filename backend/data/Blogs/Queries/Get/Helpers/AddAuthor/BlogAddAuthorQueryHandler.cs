namespace BlogHub.Data.Blogs.Get.Helpers.AddAuthor;

internal sealed class BlogAddAuthorQueryHandler : IRequestHandler<BlogAddAuthorQuery, BlogVm>
{
    private readonly IUserRepository _repository;

    public BlogAddAuthorQueryHandler(IUserRepository repository) =>
        _repository = repository;

    public async Task<BlogVm> Handle(BlogAddAuthorQuery request, CancellationToken cancellationToken)
    {
        var blog = request.Blog;

        var user = await _repository.GetAsync(request.Blog.UserId, cancellationToken);

        return blog with { Author = user?.Name };
    }
}