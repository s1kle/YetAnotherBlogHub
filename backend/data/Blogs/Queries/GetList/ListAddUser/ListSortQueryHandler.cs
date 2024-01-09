using System.Reflection;
using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Blogs.Queries.ListSort;
using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Blogs.Queries.ListAddUser;

public class ListSortQueryHandler : IRequestHandler<ListAddUserQuery, BlogListWithAuthorVm>
{
    private readonly IUserRepository _repository;

    public ListSortQueryHandler(IUserRepository repository) =>
        _repository = repository;

    public async Task<BlogListWithAuthorVm> Handle(ListAddUserQuery request, CancellationToken cancellationToken)
    {
        var blogs = request.Blogs.Blogs;

        var result = new List<BlogWithAuthorVmForList>();

        foreach(var blog in blogs)
        {
            var user = await _repository.GetAsync(blog.UserId, cancellationToken);

            result.Add(new ()
            {
                Author = user?.Name,
                Title = blog.Title,
                CreationDate = blog.CreationDate,
                Id = blog.Id
            });
        }

        return new () { Blogs = result };
    }
}