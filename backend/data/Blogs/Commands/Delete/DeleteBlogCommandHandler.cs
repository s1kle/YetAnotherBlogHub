using BlogHub.Data.Exceptions;
using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Blogs.Commands.Delete;

public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommand, Guid>
{
    private readonly IBlogRepository _repository;

    public DeleteBlogCommandHandler(IBlogRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = await _repository.GetAsync(request.Id, cancellationToken);

        if (blog is null || blog.UserId != request.UserId)
            throw new NotFoundException(nameof(blog));

        var id = await _repository.RemoveAsync(blog, cancellationToken);

        return id;
    }
}