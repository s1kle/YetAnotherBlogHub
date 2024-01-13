namespace BlogHub.Data.Users.Delete;

internal sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Guid>
{
    private readonly UserRepository _repository;

    public DeleteUserCommandHandler(UserRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetAsync(request.Id, cancellationToken);

        if (user is null) throw new NotFoundException(nameof(user));

        var id = await _repository.RemoveAsync(user, cancellationToken);

        return id;
    }
}