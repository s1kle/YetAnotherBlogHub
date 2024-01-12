
namespace BlogHub.Data.Comments.Create;

internal sealed class Handler : IRequestHandler<Command, Guid>
{
    private readonly CommentsContext.Repository _repository;

    public Handler(CommentsContext.Repository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
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