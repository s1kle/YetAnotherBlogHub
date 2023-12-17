using BlogHub.Data.Commands.Create;
using BlogHub.Domain;

namespace BlogHub.Tests.Data.Fixtures.Requests;

public class CreateCommandFixture
{
    public Blog Blog { get; }
    public CreateBlogCommand Command { get; }

    public CreateCommandFixture(Guid userId, string title, string? details)
    {
        Blog = new ()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Details = details,
            CreationDate = DateTime.UtcNow,
            EditDate = null
        };
        Command = new ()
        {
            UserId = userId,
            Title = title,
            Details = details
        };
    }
}