using BlogHub.Data.Exceptions;
using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Blogs.Commands.Update;

public class UpdateBlogCommandHandler : IRequestHandler<UpdateBlogCommand, Guid>
{
    private readonly IBlogRepository _repository;

    public UpdateBlogCommandHandler(IBlogRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        var original = await _repository.GetAsync(request.Id, cancellationToken);

        if (original is null || original.UserId != request.UserId)
            throw new NotFoundException(nameof(original));

        var updated = original with
        {
            Title = request.Title,
            Details = request.Details,
            EditDate = DateTime.UtcNow
        };

        var id = await _repository.UpdateAsync(original, updated, cancellationToken);

        return id;
    }
}
