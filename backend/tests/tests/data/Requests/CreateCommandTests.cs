using BlogHub.Data.Commands.Create;
using BlogHub.Data.Interfaces;
using BlogHub.Domain;

namespace BlogHub.Tests.Data.Requests;

public class CreateCommandTests
{
    private readonly BlogsFactory _blogsFactory;

    public CreateCommandTests()
    {
        _blogsFactory = new ();
    }

    [Fact]
    public async Task CreateBlog_ShouldSuccess()
    {
        var fixture = _blogsFactory.CreateCommandFixture("Title", null);
        (var command, var expected) = (fixture.Command, fixture.Blog);
        var actualBlogId = Guid.Empty; // ignoring expected.Id
        var repository = A.Fake<IBlogRepository>();
        A.CallTo(() => repository.CreateAsync(A<Blog>._, A<CancellationToken>._))
            .Returns(expected.Id);
        var handler = new CreateBlogCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => repository.CreateAsync(A<Blog>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Blog actual, CancellationToken token) =>
            {
                actualBlogId = actual.Id;
                actual.UserId.Should().Be(expected.UserId);
                actual.Title.Should().BeEquivalentTo(expected.Title);
                actual.CreationDate.Should().BeOnOrAfter(expected.CreationDate);
                actual.Details.Should().BeNull();
                actual.EditDate.Should().BeNull();
                return true;
            })
            .MustHaveHappenedOnceExactly();
        actualBlogId.Should().NotBeEmpty();
    }
}