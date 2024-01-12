namespace BlogHub.Data.Users.Delete;

internal sealed class Handler : IRequestHandler<Command, Guid>
{
    private readonly UsersContext.Repository _repository;

    public Handler(UsersContext.Repository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetAsync(request.Id, cancellationToken);

        if (user is null) throw new NotFoundException(nameof(user));

        var id = await _repository.RemoveAsync(user, cancellationToken);

        return id;
    }
}