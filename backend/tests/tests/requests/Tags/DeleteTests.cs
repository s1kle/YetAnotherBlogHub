namespace BlogHub.Tests.Requests.Tags;

public class DeleteTests
{
    [Fact]
    public async Task DeleteTag_WithCorrectData_ShouldSuccess()
    {
        var expected = TagFactory.CreateTag();

        var command = new DeleteTagCommand()
        {
            UserId = expected.UserId,
            Id = expected.Id
        };

        List<BlogTagLink>? links = null;

        var tagRepository = A.Fake<ITagRepository>();
        var linkRepository = A.Fake<IBlogTagRepository>();
        
        A.CallTo(() => linkRepository.GetAllAsync(A<CancellationToken>._))
            .Returns(links);
        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(expected);
        A.CallTo(() => tagRepository.RemoveAsync(A<Tag>._, A<CancellationToken>._))
            .Returns(expected.Id);

        var handler = new DeleteTagCommandHandler(tagRepository, linkRepository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => tagRepository.RemoveAsync(A<Tag>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Tag actual, CancellationToken token) =>
            {
                actual.Should().BeEquivalentTo(expected);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteTag_WithIncorrectUserId_ShouldFail()
    {
        var tag = TagFactory.CreateTag();

        var command = new DeleteTagCommand()
        {
            UserId = Guid.NewGuid(),
            Id = tag.Id
        };

        var repository = A.Fake<ITagRepository>();
        var linkRepository = A.Fake<IBlogTagRepository>();
        
        A.CallTo(() => linkRepository.GetAllAsync(A<CancellationToken>._))
            .Returns(new List<BlogTagLink>());
        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(tag);

        var handler = new DeleteTagCommandHandler(repository, linkRepository);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => repository.RemoveAsync(A<Tag>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task DeleteTag_WithIncorrectTagId_ShouldFail()
    {
        Tag? tag = null;

        var command = new DeleteTagCommand()
        {
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid()
        };

        var repository = A.Fake<ITagRepository>();
        var linkRepository = A.Fake<IBlogTagRepository>();
        
        A.CallTo(() => linkRepository.GetAllAsync(A<CancellationToken>._))
            .Returns(new List<BlogTagLink>());
        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(tag);

        var handler = new DeleteTagCommandHandler(repository, linkRepository);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => repository.RemoveAsync(A<Tag>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }
}