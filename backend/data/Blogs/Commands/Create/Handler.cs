namespace BlogHub.Data.Blogs.Create;

internal sealed class Handler : IRequestHandler<Command, Guid>
{
    private readonly BlogsContext.Repository _repository;

    public Handler(BlogsContext.Repository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
    {
        var blog = new Blog()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title,
            Details = request.Details,
            CreationDate = DateTime.UtcNow,
            EditDate = null
        };

        var id = await _repository.CreateAsync(blog, cancellationToken);

        return id;
    }
}