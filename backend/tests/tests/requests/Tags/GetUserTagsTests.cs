using BlogHub.Data.Interfaces;
using BlogHub.Data.Tags.Queries.GetList;
using BlogHub.Domain;

namespace BlogHub.Tests.Requests.Tags;

public class GetUserTagsTests
{
    [Fact]
    public async Task GetTagList_WithCorrectData_ShouldSuccess()
    {
        var size = 10;

        var tags = TagFactory.CreateTags(size, Guid.NewGuid());

        var expected = tags.Select(tag => new TagVmForList()
        {
            Id = tag.Id,
            Name = tag.Name
        }).ToList();

        var query = new GetUserTagListQuery()
        {
            UserId = tags[0].UserId,
        };

        var repository = A.Fake<ITagRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => repository.GetAllByUserIdAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(tags);
            
        A.CallTo(() => mapper.Map<TagVmForList>(A<Tag>._))
            .ReturnsNextFromSequence(expected.ToArray());

        var handler = new GetUserTagListQueryHandler(repository, mapper);

        var result = await handler.Handle(query, CancellationToken.None);

        A.CallTo(() => repository.GetAllByUserIdAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid userId, CancellationToken token) =>
            {
                userId.Should().Be(tags[0].UserId);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => mapper.Map<TagVmForList>(A<Tag>._))
            .WhenArgumentsMatch((object arg) =>
            {
                var actual = (Tag?)arg;
                tags.IndexOf(actual!).Should().NotBe(-1);
                return true;
            })
            .MustHaveHappened(size, Times.Exactly);

        var actual = result.Tags;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }
}