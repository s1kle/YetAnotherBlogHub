using BlogHub.Data.Commands.Delete;
using BlogHub.Data.Commands.Update;
using BlogHub.Data.Interfaces;
using BlogHub.Domain;

namespace BlogHub.Tests.Data.Requests;

public class DeleteCommandTests
{
    private readonly FixtureFactory _blogsFactory;

    public DeleteCommandTests()
    {
        _blogsFactory = new ();
    }

    [Fact]
    public async Task DeleteBlog_WithCorrectData_ShouldSuccess()
    {
        var fixture = _blogsFactory.DeleteCommandFixture();
        (var command, var expected) = 
            (fixture.Command, fixture.Blog);
        var repository = A.Fake<IBlogRepository>();
        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(expected);
        A.CallTo(() => repository.RemoveAsync(A<Blog>._, A<CancellationToken>._))
            .Returns(expected.Id);
        var handler = new DeleteBlogCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => repository.RemoveAsync(A<Blog>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Blog actual, CancellationToken token) =>
            {
                actual.Should().BeEquivalentTo(expected);
                return true;
            })
            .MustHaveHappenedOnceExactly();
    }
    [Fact]
    public async Task DeleteBlog_WithIncorrectUserId_ShouldFail()
    {
        var fixture = _blogsFactory.DeleteCommandFixture();
        (var command, var expected, var wrongUserId) = 
            (fixture.Command, fixture.Blog, fixture.WrongUserId);
        command = command with { UserId = wrongUserId };
        var repository = A.Fake<IBlogRepository>();
        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(expected);
        A.CallTo(() => repository.RemoveAsync(A<Blog>._, A<CancellationToken>._))
            .Returns(expected.Id);
        var handler = new DeleteBlogCommandHandler(repository);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });


        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => repository.RemoveAsync(A<Blog>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }
    [Fact]
    public async Task DeleteBlog_WithIncorrectBlogId_ShouldFail()
    {
        var fixture = _blogsFactory.DeleteCommandFixture();
        (var command, var expected) = 
            (fixture.Command, fixture.Blog);
        expected = null;
        var repository = A.Fake<IBlogRepository>();
        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(expected);
        var handler = new DeleteBlogCommandHandler(repository);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });


        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => repository.RemoveAsync(A<Blog>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }
}