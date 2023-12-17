using BlogHub.Data.Commands.Delete;
using BlogHub.Domain;

namespace BlogHub.Tests.Data.Fixtures.Requests;

public class DeleteCommandFixture
{
    public Blog Blog { get; }
    public DeleteBlogCommand Command { get; }
    public Guid WrongUserId { get; }

    public DeleteCommandFixture(Guid userId, Guid wrongUserId)
    {
        WrongUserId = wrongUserId;
        var id = Guid.NewGuid();
        Command = new ()
        {
            UserId = userId,
            Id = id,
        };
        Blog = new ()
        {
            UserId = userId,
            Id = id,
            Title = "Title",
            Details = "Details",
            CreationDate = DateTime.UtcNow,
            EditDate = null
        };
    }
}