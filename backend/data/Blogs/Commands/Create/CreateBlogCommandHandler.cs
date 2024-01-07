using BlogHub.Data.Interfaces;
using BlogHub.Domain;
using MediatR;

namespace BlogHub.Data.Blogs.Commands.Create;

public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, Guid>
{
    private readonly IBlogRepository _repository;

    public CreateBlogCommandHandler(IBlogRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = new Blog()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title,
            Details = request.Details,
            CreationDate = DateTime.UtcNow,
            EditDate = null
        };

        var id = await _repository.CreateAsync(blog, cancellationToken);

        return id;
    }
}