
namespace BlogHub.Data.Comments.Create;

internal sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Guid>
{
    private readonly ICommentRepository _repository;
    private readonly IArticleRepository _ArticleRepository;

    public CreateCommentCommandHandler(ICommentRepository repository, IArticleRepository ArticleRepository) =>
        (_repository, _ArticleRepository) = (repository, ArticleRepository);

    public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var Article = await _ArticleRepository.GetAsync(request.ArticleId, cancellationToken);

        if (Article is null) throw new NotFoundException(nameof(Article));

        var comment = new Comment()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ArticleId = request.ArticleId,
            CreationDate = DateTime.UtcNow,
            Content = request.Content
        };

        var id = await _repository.CreateAsync(comment, cancellationToken);

        return id;
    }
}