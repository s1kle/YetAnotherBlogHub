using BlogHub.Data.Commands.Update;
using BlogHub.Domain;

namespace BlogHub.Tests.Data.Fixtures.Requests;

public class UpdateCommandFixture
{
    public Blog Original { get; }
    public Blog Updated { get; }
    public UpdateBlogCommand Command { get; }
    public Guid WrongUserId { get; }

    public UpdateCommandFixture(Guid userId, Guid wrongUserId, string title, string? details, string newTitle, string newDetails)
    {
        WrongUserId = wrongUserId;
        var id = Guid.NewGuid();
        Command = new ()
        {
            UserId = userId,
            Id = id,
            Title = newTitle,
            Details = newDetails
        };
        Original = new ()
        {
            UserId = userId,
            Id = id,
            Title = title,
            Details = details,
            CreationDate = DateTime.UtcNow,
            EditDate = null
        };
        Updated = Original with
        {
            Title = newTitle,
            Details = newDetails,
            EditDate = DateTime.UtcNow,
        };
    }
}