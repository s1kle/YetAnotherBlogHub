using BlogHub.Data.Tags.Queries.Get;

namespace BlogHub.Tests.Requests.Tags;

public class GetTests
{
    [Fact]
    public async Task GetTag_WithCorrectData_ShouldSuccess()
    {
        var tag = TagFactory.CreateTag("name");

        var expected = new TagVm()
        {
            Id = tag.Id,
            Name = tag.Name,
        };

        var query = new GetTagQuery()
        {
            Id = tag.Id
        };

        var repository = A.Fake<ITagRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(tag);

        A.CallTo(() => mapper.Map<TagVm>(A<Blog>._))
            .Returns(expected);

        var handler = new GetTagQueryHandler(repository, mapper);

        var result = await handler.Handle(query, CancellationToken.None);

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(query.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => mapper.Map<TagVm>(A<Blog>._))
            .WhenArgumentsMatch((object arg) =>
            {
                var actual = (Tag)arg;
                actual.Should().BeEquivalentTo(expected);
                return true;
            })
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetTag_WithIncorrectTagId_ShouldFail()
    {
        Tag? tag = null;

        var query = new GetTagQuery()
        {
            Id = Guid.NewGuid()
        };

        var repository = A.Fake<ITagRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(tag);
            
        var handler = new GetTagQueryHandler(repository, mapper);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(query, CancellationToken.None);
        });

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => mapper.Map<TagVm>(A<Blog>._))
            .MustNotHaveHappened();
    }
}