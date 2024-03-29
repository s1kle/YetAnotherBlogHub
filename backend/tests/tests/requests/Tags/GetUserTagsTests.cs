using BlogHub.Data.Tags.Get.Helpers;

namespace BlogHub.Tests.Requests.Tags;

public class GetUserTagsTests
{
    [Fact]
    public async Task GetTagList_WithCorrectData_ShouldSuccess()
    {
        var size = 10;

        var tags = TagFactory.CreateTags(size, Guid.NewGuid());

        var expected = tags.Select(tag => new TagVm()
        {
            Id = tag.Id,
            Name = tag.Name
        }).ToList();

        var query = new GetUserTagsQuery()
        {
            UserId = tags[0].UserId,
        };

        var repository = A.Fake<ITagRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => repository.GetAllByUserIdAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(tags);

        A.CallTo(() => mapper.Map<TagVm>(A<Tag>._))
            .ReturnsNextFromSequence(expected.ToArray());

        var handler = new GetUserTagsQueryHandler(repository, mapper);

        var result = await handler.Handle(query, CancellationToken.None);

        A.CallTo(() => repository.GetAllByUserIdAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid userId, CancellationToken token) =>
            {
                userId.Should().Be(tags[0].UserId);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => mapper.Map<TagVm>(A<Tag>._))
            .WhenArgumentsMatch((object arg) =>
            {
                var actual = (Tag?)arg;
                tags.IndexOf(actual!).Should().NotBe(-1);
                return true;
            })
            .MustHaveHappened(size, Times.Exactly);

        var actual = result;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }
}