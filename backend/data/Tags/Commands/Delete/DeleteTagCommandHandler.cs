using BlogHub.Data.Exceptions;
using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Tags.Commands.Delete;

public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, Guid>
{
    private readonly ITagRepository _repository;

    public DeleteTagCommandHandler(ITagRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _repository.GetAsync(request.Id, cancellationToken);

        if (tag is null || tag.UserId != request.UserId)
            throw new NotFoundException(nameof(tag));

        var id = await _repository.RemoveAsync(tag, cancellationToken);

        return id;
    }
}