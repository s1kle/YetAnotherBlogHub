
namespace BlogHub.Data.Comments.Create;

internal sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Guid>
{
    private readonly CommentRepository _repository;

    public CreateCommentCommandHandler(CommentRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
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