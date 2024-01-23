namespace BlogHub.Data.Articles.Update;

internal sealed class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, Guid>
{
    private readonly IArticleRepository _repository;

    public UpdateArticleCommandHandler(IArticleRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
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
