using BlogHub.Data.Blogs.Commands.Update;
using BlogHub.Data.Exceptions;
using BlogHub.Data.Interfaces;
using BlogHub.Domain;

namespace BlogHub.Tests.Requests;

public class UpdateTests
{
    [Fact]
    public async Task UpdateBlog_WithCorrectData_ShouldSuccess()
    {
        var blog = BlogFactory.CreateBlog("title");

        var expected = new Blog()
        {
            UserId = blog.UserId,
            Id = blog.Id,
            Title = "New Title",
            CreationDate = blog.CreationDate,
            Details = "Details",
            EditDate = DateTime.UtcNow
        };

        var command = new UpdateBlogCommand()
        {
            UserId = blog.UserId,
            Id = blog.Id,
            Title = "New Title",
            Details = "Details"
        };

        var repository = A.Fake<IBlogRepository>();

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(blog);
        A.CallTo(() => repository.UpdateAsync(A<Blog>._, A<Blog>._, A<CancellationToken>._))
            .Returns(blog.Id);

        var handler = new UpdateBlogCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => repository.UpdateAsync(A<Blog>._, A<Blog>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Blog original, Blog actual, CancellationToken token) =>
            {
                original.Should().BeEquivalentTo(blog);

                actual.UserId.Should().Be(expected.UserId);
                actual.Title.Should().BeEquivalentTo(expected.Title);
                actual.CreationDate.Should().BeOneOf(expected.CreationDate);
                actual.Details.Should().BeEquivalentTo(expected.Details);
                actual.EditDate.Should().BeOnOrAfter(expected.EditDate.Value);

                return true;
            })
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateBlog_WithIncorrectBlogId_ShouldFail()
    {
        Blog? blog = null;

        var command = new UpdateBlogCommand()
        {
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid(),
            Title = "New Title",
            Details = "Details"
        };

        var repository = A.Fake<IBlogRepository>();

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(blog);

        var handler = new UpdateBlogCommandHandler(repository);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => repository.UpdateAsync(A<Blog>._, A<Blog>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UpdateBlog_WithIncorrectUserId_ShouldFail()
    {
        var blog = BlogFactory.CreateBlog("title");

        var command = new UpdateBlogCommand()
        {
            UserId = Guid.NewGuid(),
            Id = blog.Id,
            Title = "New Title",
            Details = "Details"
        };

        var repository = A.Fake<IBlogRepository>();

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(blog);

        var handler = new UpdateBlogCommandHandler(repository);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => repository.UpdateAsync(A<Blog>._, A<Blog>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }
}