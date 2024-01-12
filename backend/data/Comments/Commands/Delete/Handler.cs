namespace BlogHub.Data.Comments.Delete;

internal sealed class Handler : IRequestHandler<Command, Guid>
{
    private readonly CommentsContext.Repository _repository;

    public Handler(CommentsContext.Repository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
    {
        var comment = await _repository.GetAsync(request.Id, cancellationToken);

        if (comment is null || comment.UserId != request.UserId) throw new NotFoundException(nameof(comment));

        var id = await _repository.RemoveAsync(comment, cancellationToken);

        return id;
    }
}