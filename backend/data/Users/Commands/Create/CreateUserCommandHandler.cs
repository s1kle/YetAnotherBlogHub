namespace BlogHub.Data.Users.Commands.Create;

internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _repository;

    public CreateUserCommandHandler(IUserRepository repository) =>
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