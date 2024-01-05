using BlogHub.Data.Interfaces;
using BlogHub.Data.Queries.GetList;
using BlogHub.Domain;

namespace BlogHub.Tests.Data.Requests;

public class GetListQueryTests
{
    private readonly FixtureFactory _blogsFactory;

    public GetListQueryTests()
    {
        _blogsFactory = new ();
    }

    [Fact]
    public async Task GetBlogList_WithCorrectData_ShouldSuccess()
    {
        int size = 10;
        var fixture = _blogsFactory.GetBlogListFixture(size);
        (var query, var blogList, var blogVmList, var blogListVm) = 
            (fixture.Query, fixture.BlogList, fixture.BlogVmList, fixture.BlogListVm);
        var repository = A.Fake<IBlogRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => repository.GetAllByUserIdAsync(A<Guid>._, A<int>._, A<int>._, A<CancellationToken>._))
            .Returns(blogList);
        A.CallTo(() => mapper.Map<BlogVmForList>(A<Blog>._))
            .ReturnsNextFromSequence(blogVmList.ToArray());

        var handler = new GetBlogListQueryHandler(repository, mapper);

        var result = await handler.Handle(query, CancellationToken.None);

        A.CallTo(() => repository.GetAllByUserIdAsync(A<Guid>._, A<int>._, A<int>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => mapper.Map<BlogVmForList>(A<Blog>._))
            .WhenArgumentsMatch(actualArgument =>
            {
                var actual = (Blog?)actualArgument.FirstOrDefault();
                var index = 
                    blogList.IndexOf(actual ?? throw new ArgumentNullException($"Blog List dont contains {actual}"));
                index.Should().NotBe(-1);
                return true;
            })
            .MustHaveHappened(size, Times.Exactly);

        var actualBlogs = result.Blogs;
        for(var i = 0; i < size; i++)
        {
            actualBlogs[i].Should().BeEquivalentTo(blogVmList[i]);
        }
    }
}