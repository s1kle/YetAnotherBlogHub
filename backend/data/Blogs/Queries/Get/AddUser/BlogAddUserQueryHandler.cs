using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Blogs.Queries.Get;

public class BlogAddUserQueryHandler : IRequestHandler<BlogAddUserQuery, BlogWithAuthorVm>
{
    private readonly IUserRepository _repository;

    public BlogAddUserQueryHandler(IUserRepository repository) =>
        _repository = repository;

    public async Task<BlogWithAuthorVm> Handle(BlogAddUserQuery request, CancellationToken cancellationToken)
    {
        var blog = request.Blog;

        var user = await _repository.GetAsync(request.Blog.UserId, cancellationToken);

        return new()
        {
            Author = user?.Name,
            Id = blog.Id,
            Title = blog.Title,
            Details = blog.Details,
            CreationDate = blog.CreationDate,
            EditDate = blog.EditDate
        };
    }
}