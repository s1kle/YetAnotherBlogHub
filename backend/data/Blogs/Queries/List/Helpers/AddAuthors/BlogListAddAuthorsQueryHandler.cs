namespace BlogHub.Data.Blogs.List.Helpers.AddAuthors;

internal sealed class BlogListAddAuthorsQueryHandler : IRequestHandler<BlogListAddAuthorsQuery, BlogListVm>
{
    private readonly UserRepository _repository;

    public BlogListAddAuthorsQueryHandler(UserRepository repository) =>
        _repository = repository;

    public async Task<BlogListVm> Handle(BlogListAddAuthorsQuery request, CancellationToken cancellationToken)
    {
        var blogs = request.Blogs.Blogs;

        var result = blogs.ToList();

        for (var i = 0; i < blogs.Count; i++)
        {
            var blog = result[i];
            var user = await _repository.GetAsync(blog.UserId, cancellationToken);
            result[i] = blog with { Author = user?.Name };
        }

        return new() { Blogs = result };
    }
}