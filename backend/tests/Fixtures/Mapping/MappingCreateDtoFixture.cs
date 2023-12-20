using BlogHub.Data.Commands.Create;

namespace BlogHub.Tests.Fixtures.Mapping;

public class MappingCreateDtoFixture
{   
    public CreateBlogDto Dto { get; }
    public CreateBlogCommand Command { get; }
    public Guid UserId { get; }
    public MappingCreateDtoFixture(Guid userId, string title, string? details)
    {
        UserId = userId;
        Dto = new ()
        {
            Title = title,
            Details = details,
        };
        Command = new ()
        {
            UserId = userId,
            Title = title,
            Details = details
        };
    }
}