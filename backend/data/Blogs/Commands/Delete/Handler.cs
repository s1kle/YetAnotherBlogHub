namespace BlogHub.Data.Blogs.Delete;

internal sealed class Handler : IRequestHandler<Command, Guid>
{
    private readonly BlogsContext.Repository _repository;

    public Handler(BlogsContext.Repository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
    {
        var blog = await _repository.GetAsync(request.Id, cancellationToken);

        if (blog is null || blog.UserId != request.UserId)
            throw new NotFoundException(nameof(blog));

        var id = await _repository.RemoveAsync(blog, cancellationToken);

        return id;
    }
}