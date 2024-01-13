namespace BlogHub.Data.Tags.Create;

internal sealed class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Guid>
{
    private readonly TagRepository _repository;

    public CreateTagCommandHandler(TagRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(CreateTagCommand request, CancellationToken cancellationToken)
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