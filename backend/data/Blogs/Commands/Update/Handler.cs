namespace BlogHub.Data.Blogs.Update;

internal sealed class Handler : IRequestHandler<Command, Guid>
{
    private readonly BlogsContext.Repository _repository;

    public Handler(BlogsContext.Repository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
    {
        var original = await _repository.GetAsync(request.Id, cancellationToken);

        if (original is null || original.UserId != request.UserId)
            throw new NotFoundException(nameof(original));

        var updated = original with
        {
            Title = request.Title,
            Details = request.Details,
            EditDate = DateTime.UtcNow
        };

        var id = await _repository.UpdateAsync(original, updated, cancellationToken);

        return id;
    }
}
