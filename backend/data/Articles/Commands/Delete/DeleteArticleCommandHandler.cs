namespace BlogHub.Data.Articles.Delete;

internal sealed class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, Guid>
{
    private readonly IArticleRepository _repository;

    public DeleteArticleCommandHandler(IArticleRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await _repository.GetAsync(request.Id, cancellationToken);

        if (article is null || article.UserId != request.UserId)
            throw new NotFoundException(nameof(article));

        var id = await _repository.RemoveAsync(article, cancellationToken);

        return id;
    }
}