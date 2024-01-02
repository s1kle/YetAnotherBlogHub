using BlogHub.Data.Interfaces;
using BlogHub.Data.Queries.Get;
using BlogHub.Domain;

namespace BlogHub.Tests.Data.Requests;

public class GetQueryTests
{
    private readonly FixtureFactory _blogsFactory;

    public GetQueryTests()
    {
        _blogsFactory = new ();
    }

    [Fact]
    public async Task GetBlog_WithCorrectData_ShouldSuccess()
    {
        var fixture = _blogsFactory.GetBlogFixture();
        (var query, var expected, var expectedVm) = 
            (fixture.Query, fixture.Blog, fixture.BlogVm);
        var repository = A.Fake<IBlogRepository>();
        var mapper = A.Fake<IMapper>();
        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._)).Returns(expected);
        A.CallTo(() => mapper.Map<BlogVm>(A<Blog>._)).Returns(expectedVm);
        var handler = new GetBlogQueryHandler(repository, mapper);

        var result = await handler.Handle(query, CancellationToken.None);

        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => mapper.Map<BlogVm>(A<Blog>._))
            .WhenArgumentsMatch(actualArgument =>
            {
                var actual = (Blog?)actualArgument.FirstOrDefault();
                actual.Should().BeEquivalentTo(expected);
                return true;
            })
            .MustHaveHappenedOnceExactly();
        result.Should().BeEquivalentTo(expectedVm);
    }

    [Fact]
    public async Task GetBlog_WithIncorrectBlogId_ShouldFail()
    {
        var fixture = _blogsFactory.GetBlogFixture();
        (var query, var expected, var expectedVm) = 
            (fixture.Query, fixture.Blog, fixture.BlogVm);
        var repository = A.Fake<IBlogRepository>();
        var mapper = A.Fake<IMapper>();
        expected = null;
        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._)).Returns(expected);
        A.CallTo(() => mapper.Map<BlogVm>(A<Blog>._)).Returns(expectedVm);
        var handler = new GetBlogQueryHandler(repository, mapper);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            var result = await handler.Handle(query, CancellationToken.None);
        });

        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => mapper.Map<BlogVm>(A<Blog>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task GetBlog_WithIncorrectUserId_ShouldFail()
    {
        var fixture = _blogsFactory.GetBlogFixture();
        (var query, var expected, var expectedVm, var wrongUserId) = 
            (fixture.Query, fixture.Blog, fixture.BlogVm, fixture.WrongUserId);
        var repository = A.Fake<IBlogRepository>();
        var mapper = A.Fake<IMapper>();
        query = query with { UserId = wrongUserId };
        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._)).Returns(expected);
        A.CallTo(() => mapper.Map<BlogVm>(A<Blog>._)).Returns(expectedVm);
        var handler = new GetBlogQueryHandler(repository, mapper);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            var result = await handler.Handle(query, CancellationToken.None);
        });

        A.CallTo(() => repository.GetBlogAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => mapper.Map<BlogVm>(A<Blog>._))
            .MustNotHaveHappened();
    }
}