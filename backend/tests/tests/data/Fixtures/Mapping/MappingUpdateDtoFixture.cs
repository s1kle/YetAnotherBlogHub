using BlogHub.Data.Commands.Update;

namespace BlogHub.Tests.Data.Fixtures.Mapping;

public class MappingUpdateDtoFixture
{   
    public UpdateBlogDto Dto { get; }
    public UpdateBlogCommand Command { get; }
    public Guid UserId { get; }
    public Guid Id { get; }
    public MappingUpdateDtoFixture(Guid userId, string title, string? details)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Dto = new ()
        {
            Title = title,
            Details = details,
        };
        Command = new ()
        {
            Id = Id,
            UserId = userId,
            Title = title,
            Details = details
        };
    }
}