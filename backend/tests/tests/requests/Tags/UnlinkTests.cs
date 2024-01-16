namespace BlogHub.Tests.Requests.Tags;

public class UninkTests
{
    [Fact]
    public async Task UnlinkTag_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var Article = ArticleFactory.CreateArticle(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        var expected = LinkFactory.CreateArticleTagLink(Article, tag);

        var command = new UnlinkTagCommand()
        {
            UserId = userId,
            TagId = tag.Id,
            ArticleId = Article.Id
        };

        var tagRepository = A.Fake<ITagRepository>();
        var linkRepository = A.Fake<IArticleTagRepository>();
        var ArticleRepository = A.Fake<IArticleRepository>();

        A.CallTo(() => ArticleRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(Article);
        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(tag);
        A.CallTo(() => linkRepository.GetAsync(A<Guid>._, A<Guid>._, A<CancellationToken>._))
            .Returns(expected);
        A.CallTo(() => linkRepository.RemoveAsync(A<ArticleTag>._, A<CancellationToken>._))
            .Returns(expected.Id);

        var handler = new UnlinkTagCommandHandler(tagRepository, linkRepository, ArticleRepository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(tag.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => linkRepository.GetAsync(A<Guid>._, A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid ArticleId, Guid tagId, CancellationToken token) =>
            {
                tagId.Should().Be(tag.Id);
                ArticleId.Should().Be(Article.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => ArticleRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(Article.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => linkRepository.RemoveAsync(A<ArticleTag>._, A<CancellationToken>._))
            .WhenArgumentsMatch((ArticleTag actual, CancellationToken token) =>
            {
                actual.Should().BeEquivalentTo(expected);
                token.Should().Be(CancellationToken.None);

                return true;
            })
            .MustHaveHappenedOnceExactly();

        result.Should().Be(expected.Id);
    }

    [Fact]
    public async Task UnlinkTag_WithIncorrectTagId_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var Article = ArticleFactory.CreateArticle(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        ArticleTag? temp = null;
        var expected = LinkFactory.CreateArticleTagLink(Article, tag);

        var command = new UnlinkTagCommand()
        {
            UserId = userId,
            TagId = Guid.NewGuid(),
            ArticleId = Article.Id
        };

        tag = null;

        var tagRepository = A.Fake<ITagRepository>();
        var linkRepository = A.Fake<IArticleTagRepository>();
        var ArticleRepository = A.Fake<IArticleRepository>();

        A.CallTo(() => ArticleRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(Article);
        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(tag);
        A.CallTo(() => linkRepository.GetAsync(A<Guid>._, A<Guid>._, A<CancellationToken>._))
            .Returns(temp);

        A.CallTo(() => linkRepository.RemoveAsync(A<ArticleTag>._, A<CancellationToken>._))
            .Returns(expected.Id);

        var handler = new UnlinkTagCommandHandler(tagRepository, linkRepository, ArticleRepository);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.TagId);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => linkRepository.GetAsync(A<Guid>._, A<Guid>._, A<CancellationToken>._))
            .MustNotHaveHappened();
        A.CallTo(() => ArticleRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .MustNotHaveHappened();
        A.CallTo(() => linkRepository.RemoveAsync(A<ArticleTag>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UnlinkTag_WithIncorrectArticleId_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var Article = ArticleFactory.CreateArticle(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        ArticleTag? temp = null;
        var expected = LinkFactory.CreateArticleTagLink(Article, tag);

        var command = new UnlinkTagCommand()
        {
            UserId = userId,
            TagId = tag.Id,
            ArticleId = Guid.NewGuid()
        };

        Article = null;

        var tagRepository = A.Fake<ITagRepository>();
        var linkRepository = A.Fake<IArticleTagRepository>();
        var ArticleRepository = A.Fake<IArticleRepository>();

        A.CallTo(() => ArticleRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(Article);
        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(tag);
        A.CallTo(() => linkRepository.GetAsync(A<Guid>._, A<Guid>._, A<CancellationToken>._))
            .Returns(temp);

        A.CallTo(() => linkRepository.RemoveAsync(A<ArticleTag>._, A<CancellationToken>._))
            .Returns(expected.Id);

        var handler = new UnlinkTagCommandHandler(tagRepository, linkRepository, ArticleRepository);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.TagId);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => ArticleRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.ArticleId);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => linkRepository.GetAsync(A<Guid>._, A<Guid>._, A<CancellationToken>._))
            .MustNotHaveHappened();
        A.CallTo(() => linkRepository.RemoveAsync(A<ArticleTag>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UnlinkTag_WithIncorrectLinkId_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var Article = ArticleFactory.CreateArticle(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        var expected = LinkFactory.CreateArticleTagLink(Article, tag);

        var command = new UnlinkTagCommand()
        {
            UserId = userId,
            TagId = tag.Id,
            ArticleId = Guid.NewGuid()
        };

        expected = null;

        var tagRepository = A.Fake<ITagRepository>();
        var linkRepository = A.Fake<IArticleTagRepository>();
        var ArticleRepository = A.Fake<IArticleRepository>();

        A.CallTo(() => ArticleRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(Article);
        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(tag);
        A.CallTo(() => linkRepository.GetAsync(A<Guid>._, A<Guid>._, A<CancellationToken>._))
            .Returns(expected);
        A.CallTo(() => linkRepository.RemoveAsync(A<ArticleTag>._, A<CancellationToken>._))
            .Returns(Guid.Empty);

        var handler = new UnlinkTagCommandHandler(tagRepository, linkRepository, ArticleRepository);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.TagId);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => ArticleRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.ArticleId);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => linkRepository.GetAsync(A<Guid>._, A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid ArticleId, Guid tagId, CancellationToken token) =>
            {
                tagId.Should().Be(tag.Id);
                ArticleId.Should().Be(Article.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => linkRepository.RemoveAsync(A<ArticleTag>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }
}