using BlogHub.Data.Commands.Update;
using BlogHub.Data.Interfaces;
using BlogHub.Domain;

namespace BlogHub.Tests.Data.Requests;

public class UpdateCommandTests
{
    private readonly BlogsFactory _blogsFactory;

    public UpdateCommandTests()
    {
        _blogsFactory = new ();
    }

    [Fact]
    public async Task UpdateBlog_WithCorrectData_ShouldSuccess()
    {
        var fixture = _blogsFactory.UpdateCommandFixture("Title", "Details", "New Title", "New Details");
        (var command, var expectedOriginal, var expectedUpdated) = 
            (fixture.Command, fixture.Original, fixture.Updated);
        var repository = A.Fake<IBlogRepository>();
        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(expectedOriginal);
        A.CallTo(() => repository.UpdateAsync(A<Blog>._, A<Blog>._, A<CancellationToken>._))
            .Returns(expectedOriginal.Id);
        var handler = new UpdateBlogCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => repository.UpdateAsync(A<Blog>._, A<Blog>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Blog actualOriginal, Blog actualUpdated, CancellationToken token) =>
            {
                #region  original
                actualOriginal.Should().BeEquivalentTo(expectedOriginal);
                #endregion
                #region updated
                actualUpdated.Id.Should().Be(expectedUpdated.Id);
                actualUpdated.UserId.Should().Be(expectedUpdated.UserId);
                actualUpdated.Title.Should().BeEquivalentTo(expectedUpdated.Title);
                actualUpdated.Details.Should().BeEquivalentTo(expectedUpdated.Details);
                actualUpdated.CreationDate.Should().BeSameDateAs(expectedUpdated.CreationDate);
                actualUpdated.EditDate.Should().BeOnOrAfter(actualUpdated.EditDate ?? 
                    throw new NullReferenceException($"{nameof(actualUpdated.EditDate)} should not be null"));
                #endregion
                return true;
            })
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateBlog_WithIncorrectBlogId_ShouldFail()
    {
        var fixture = _blogsFactory.UpdateCommandFixture("Title", "Details", "New Title", "New Details");
        (var command, var expectedOriginal, var expectedUpdated) = 
            (fixture.Command, fixture.Original, fixture.Updated);
        expectedOriginal = null;
        var repository = A.Fake<IBlogRepository>();
        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(expectedOriginal);
        var handler = new UpdateBlogCommandHandler(repository);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => repository.UpdateAsync(A<Blog>._, A<Blog>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UpdateBlog_WithIncorrectUserId_ShouldFail()
    {
        var fixture = _blogsFactory.UpdateCommandFixture("Title", "Details", "New Title", "New Details");
        (var command, var expectedOriginal, var expectedUpdated, var wrongUserId) = 
            (fixture.Command, fixture.Original, fixture.Updated, fixture.WrongUserId);
        command = command with { UserId = wrongUserId };
        var repository = A.Fake<IBlogRepository>();
        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(expectedOriginal);
        var handler = new UpdateBlogCommandHandler(repository);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => repository.UpdateAsync(A<Blog>._, A<Blog>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }
}