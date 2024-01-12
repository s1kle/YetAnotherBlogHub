
namespace BlogHub.Data.Comments.Commands.Create;

internal sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Guid>
{
    private readonly ICommentRepository _repository;

    public CreateCommentCommandHandler(ICommentRepository repository) =>
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