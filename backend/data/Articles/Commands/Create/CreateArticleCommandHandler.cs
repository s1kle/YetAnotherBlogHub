namespace BlogHub.Data.Articles.Create;

internal sealed class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, Guid>
{
    private readonly IArticleRepository _repository;

    public CreateArticleCommandHandler(IArticleRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var article = new Article()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title,
            Details = request.Details,
            CreationDate = DateTime.UtcNow,
            EditDate = null
        };

        var id = await _repository.CreateAsync(article, cancellationToken);

        return id;
    }
}