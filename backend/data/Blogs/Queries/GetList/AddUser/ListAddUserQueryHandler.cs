namespace BlogHub.Data.Blogs.Queries.GetList.AddUser;

internal sealed class ListAddUserQueryHandler : IRequestHandler<ListAddUserQuery, BlogListVm>
{
    private readonly IUserRepository _repository;

    public ListAddUserQueryHandler(IUserRepository repository) =>
        _repository = repository;

    public async Task<BlogListVm> Handle(ListAddUserQuery request, CancellationToken cancellationToken)
    {
        var blogs = request.Blogs.Blogs;

        var result = blogs.ToList();

        for (var i = 0; i < blogs.Count; i++)
        {
            var blog = result[i];
            var user = await _repository.GetAsync(blog.UserId, cancellationToken);
            result[i] = blog with { Author = user?.Name };
        }

        return new () { Blogs = result };
    }
}