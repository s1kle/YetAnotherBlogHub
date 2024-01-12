namespace BlogHub.Data.Blogs.Queries.GetList.AddUser;

internal sealed class ListAddUserQueryHandler : IRequestHandler<ListAddUserQuery, BlogListVm>
{
    private readonly IUserRepository _repository;

    public ListAddUserQueryHandler(IUserRepository repository) =>
        _repository = repository;

    public async Task<BlogListVm> Handle(ListAddUserQuery request, CancellationToken cancellationToken)
    {
        var blogs = request.Blogs.Blogs;

        foreach(var blog in blogs)
        {
            var user = await _repository.GetAsync(blog.UserId, cancellationToken);

            blog.Author = user?.Name;
        }

        return new () { Blogs = blogs };
    }
}