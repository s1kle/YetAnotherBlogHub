namespace BlogHub.Tests.Data.Fixtures;

public class BlogsFactory
{
    public readonly Guid FirstUserId;
    public readonly Guid SecondUserId;

    public BlogsFactory()
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
}