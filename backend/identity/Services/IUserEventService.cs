using BlogHub.Domain.UserEvents;

namespace BlogHub.Identity.Services;

public interface IUserEventService
{
    void Publish(UserCreatedEvent userCreatedEvent);
    void Publish(UserDeletedEvent userDeletedEvent);
}