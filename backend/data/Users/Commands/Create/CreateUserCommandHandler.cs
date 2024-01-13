namespace BlogHub.Data.Users.Create;

internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly UserRepository _repository;

    public CreateUserCommandHandler(UserRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
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