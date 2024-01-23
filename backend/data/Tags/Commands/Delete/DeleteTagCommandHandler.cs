namespace BlogHub.Data.Tags.Delete;

internal sealed class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, Guid>
{
    private readonly ITagRepository _repository;

    public DeleteTagCommandHandler(ITagRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _repository.GetAsync(request.Id, cancellationToken);

        if (tag is null || tag.UserId != request.UserId)
            throw new NotFoundException(nameof(tag));

        var id = await _repository.RemoveAsync(tag, cancellationToken);

        return id;
    }
}