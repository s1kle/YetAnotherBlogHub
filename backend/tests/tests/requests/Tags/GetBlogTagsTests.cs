using BlogHub.Data.Interfaces;
using BlogHub.Data.Tags.Queries.GetList;
using BlogHub.Domain;

namespace BlogHub.Tests.Requests.Tags;

public class GetBlogTagsTests
{
    [Fact]
    public async Task GetTagList_WithCorrectData_ShouldSuccess()
    {
        var size = 10;

        var temp = TagFactory.CreateTags(size, Guid.NewGuid());
        var blog = BlogFactory.CreateBlog();

        var links = temp
            .Select(tag => LinkFactory.CreateBlogTagLink(blog, tag))
            .ToList();

        var tags = links
            .Select(link => temp.FirstOrDefault(tag => tag.Id.Equals(link.TagId)));

        var expected = tags
            .Select(tag => new TagVmForList()
            {
                Id = tag?.Id ?? throw new ArgumentNullException("Tag is null"),
                Name = tag.Name
            }).ToList();

        var query = new GetBlogTagListQuery()
        {
            BlogId = blog.Id
        };

        var tagRepository = A.Fake<ITagRepository>();
        var linkRepository = A.Fake<IBlogTagRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .ReturnsNextFromSequence(tags.ToArray());
        A.CallTo(() => linkRepository.GetAllByBlogIdAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(links);
        A.CallTo(() => mapper.Map<TagVmForList>(A<Tag>._))
            .ReturnsNextFromSequence(expected.ToArray());

        var handler = new GetBlogTagListQueryHandler(tagRepository, linkRepository, mapper);

        var result = await handler.Handle(query, CancellationToken.None);

        A.CallTo(() => linkRepository.GetAllByBlogIdAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid blogId, CancellationToken token) =>
            {
                blogId.Should().Be(query.BlogId);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                temp.Any(tag => tag.Id.Equals(id)).Should().BeTrue();
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappened(size, Times.Exactly);

        A.CallTo(() => mapper.Map<TagVmForList>(A<Tag>._))
            .WhenArgumentsMatch((object arg) =>
            {
                var actual = (Tag?)arg;
                temp.IndexOf(actual!).Should().NotBe(-1);
                return true;
            })
            .MustHaveHappened(size, Times.Exactly);

        var actual = result.Tags;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }
}