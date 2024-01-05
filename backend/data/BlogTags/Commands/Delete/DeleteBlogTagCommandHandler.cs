using BlogHub.Data.Exceptions;
using BlogHub.Data.Interfaces;
using BlogHub.Domain;
using MediatR;

namespace BlogHub.Data.BlogTags.Commands.Delete;

public class CreateBlogTagCommandHandler : IRequestHandler<DeleteBlogTagCommand, Guid>
{
    private readonly IBlogTagRepository _repository;

    public CreateBlogTagCommandHandler(IBlogTagRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(DeleteBlogTagCommand request, CancellationToken cancellationToken)
    {
        var blogTag = await _repository.GetAsync(request.Id, cancellationToken);

        if (blogTag is null || request.UserId != blogTag.UserId) throw new NotFoundException(nameof(blogTag));

        var id = await _repository.RemoveAsync(blogTag, cancellationToken);

        return id;
    }
}