namespace BlogHub.Data.Comments.Delete;

internal sealed class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Guid>
{
    private readonly ICommentRepository _repository;

    public DeleteCommentCommandHandler(ICommentRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _repository.GetAsync(request.Id, cancellationToken);

        if (comment is null || comment.UserId != request.UserId) throw new NotFoundException(nameof(comment));

        var id = await _repository.RemoveAsync(comment, cancellationToken);

        return id;
    }
}