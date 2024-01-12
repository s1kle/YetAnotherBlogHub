namespace BlogHub.Data.Users.Create;

internal sealed class Handler : IRequestHandler<Command, Guid>
{
    private readonly UsersContext.Repository _repository;

    public Handler(UsersContext.Repository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            Id = request.Id,
            Name = request.Name
        };

        var id = await _repository.CreateAsync(user, cancellationToken);

        return id;
    }
}