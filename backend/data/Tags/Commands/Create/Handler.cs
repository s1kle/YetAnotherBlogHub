namespace BlogHub.Data.Tags.Create;

internal sealed class Handler : IRequestHandler<Command, Guid>
{
    private readonly TagsContext.Repository _repository;

    public Handler(TagsContext.Repository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
    {
        var tag = new Tag()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Name = request.Name
        };

        var id = await _repository.CreateAsync(tag, cancellationToken);

        return id;
    }
}