using BlogHub.Tests.Fixtures.Mapping;
using BlogHub.Tests.Fixtures.Requests;
using BlogHub.Tests.Fixtures.Validation;

namespace BlogHub.Tests.Fixtures;

public class FixtureFactory
{
    public readonly Guid FirstUserId;
    public readonly Guid SecondUserId;

    public FixtureFactory()
    {
        FirstUserId = Guid.Parse("7fa2bd57-76bc-502b-a62a-7fb34b6ac4ae");
        SecondUserId = Guid.Parse("58e19cd0-ea9e-5f8f-b26c-ba61148759de");
    }

    public CreateCommandFixture CreateCommandFixture(string title, string? details) =>
        new (FirstUserId, title, details);
    public UpdateCommandFixture UpdateCommandFixture(string title, string? details, string newTitle, string newDetails) =>
        new (FirstUserId, SecondUserId, title, details, newTitle, newDetails);
    public DeleteCommandFixture DeleteCommandFixture() =>
        new (FirstUserId, SecondUserId);
    public GetBlogFixture GetBlogFixture() =>
        new (FirstUserId, SecondUserId);
    public GetBlogListFixture GetBlogListFixture(int size) =>
        new (FirstUserId, size);
    public MappingCreateDtoFixture MappingCreateDtoFixture(string title, string? details) =>
        new (FirstUserId, title, details);
    public MappingUpdateDtoFixture MappingUpdateDtoFixture(string title, string? details) =>
        new (FirstUserId, title, details);
    public ValidationFixture ValidationFixture() =>
        new ();
}