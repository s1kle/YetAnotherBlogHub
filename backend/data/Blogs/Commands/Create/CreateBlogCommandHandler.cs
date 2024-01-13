namespace BlogHub.Data.Blogs.Create;

internal sealed class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, Guid>
{
    private readonly BlogRepository _repository;

    public CreateBlogCommandHandler(BlogRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
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