using BlogHub.Data.Exceptions;
using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Users.Commands.Delete;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Guid>
{
    private readonly IUserRepository _repository;

    public DeleteUserCommandHandler(IUserRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetAsync(request.Id, cancellationToken);

        if (user is null) throw new NotFoundException(nameof(user));

        var id = await _repository.RemoveAsync(user, cancellationToken);

        return id;
    }
}