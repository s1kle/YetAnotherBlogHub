
namespace BlogHub.Data.Comments.Create;

internal sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Guid>
{
    private readonly ICommentRepository _repository;
    private readonly IBlogRepository _blogRepository;

    public CreateCommentCommandHandler(ICommentRepository repository, IBlogRepository blogRepository) =>
        (_repository, _blogRepository) = (repository, blogRepository);

    public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.GetAsync(request.BlogId, cancellationToken);

        if (blog is null) throw new NotFoundException(nameof(blog));

        var comment = new Comment()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            BlogId = request.BlogId,
            CreationDate = DateTime.UtcNow,
            Content = request.Content
        };

        var id = await _repository.CreateAsync(comment, cancellationToken);

        return id;
    }
}