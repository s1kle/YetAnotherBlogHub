using BlogHub.Data.Tags.Queries.Get;
using BlogHub.Data.Tags.Queries.GetList;

namespace BlogHub.Tests.Requests.Tags;

public class GetAllTests
{
    [Fact]
    public async Task GetTagList_WithCorrectData_ShouldSuccess()
    {
        var size = 10;

        var tags = TagFactory.CreateTags(size);

        var expected = tags.Select(tag => new TagVm()
        {
            Id = tag.Id,
            Name = tag.Name
        }).ToList();

        var query = new GetTagListQuery();

        var repository = A.Fake<ITagRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => repository.GetAllAsync(A<CancellationToken>._))
            .Returns(tags);
            
        A.CallTo(() => mapper.Map<TagVm>(A<Tag>._))
            .ReturnsNextFromSequence(expected.ToArray());

        var handler = new GetTagListQueryHandler(repository, mapper);

        var result = await handler.Handle(query, CancellationToken.None);

        A.CallTo(() => repository.GetAllAsync(A<CancellationToken>._))
            .WhenArgumentsMatch((CancellationToken token) =>
            {
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