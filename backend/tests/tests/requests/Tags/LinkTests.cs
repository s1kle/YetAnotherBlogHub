namespace BlogHub.Tests.Requests.Tags;

public class LinkTests
{
    [Fact]
    public async Task LinkTag_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var Article = ArticleFactory.CreateArticle(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        ArticleTagLink? temp = null;
        var expected = LinkFactory.CreateArticleTagLink(Article, tag);

        var command = new LinkTagCommand()
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
            .Returns(temp);

        A.CallTo(() => linkRepository.CreateAsync(A<ArticleTagLink>._, A<CancellationToken>._))
            .Returns(expected.Id);

        var handler = new LinkTagCommandHandler(tagRepository, linkRepository, ArticleRepository);

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

        A.CallTo(() => linkRepository.CreateAsync(A<ArticleTagLink>._, A<CancellationToken>._))
            .WhenArgumentsMatch((ArticleTagLink actual, CancellationToken token) =>
            {
                actual.ArticleId.Should().Be(expected.ArticleId);
                actual.TagId.Should().Be(expected.TagId);

                token.Should().Be(CancellationToken.None);

                return true;
            })
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task LinkTag_WithIncorrectTagId_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var Article = ArticleFactory.CreateArticle(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        ArticleTagLink? temp = null;
        var expected = LinkFactory.CreateArticleTagLink(Article, tag);

        var command = new LinkTagCommand()
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

        A.CallTo(() => linkRepository.CreateAsync(A<ArticleTagLink>._, A<CancellationToken>._))
            .Returns(expected.Id);

        var handler = new LinkTagCommandHandler(tagRepository, linkRepository, ArticleRepository);

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
        A.CallTo(() => linkRepository.CreateAsync(A<ArticleTagLink>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task LinkTag_WithIncorrectArticleId_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var Article = ArticleFactory.CreateArticle(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        ArticleTagLink? temp = null;
        var expected = LinkFactory.CreateArticleTagLink(Article, tag);

        var command = new LinkTagCommand()
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

        A.CallTo(() => linkRepository.CreateAsync(A<ArticleTagLink>._, A<CancellationToken>._))
            .Returns(expected.Id);

        var handler = new LinkTagCommandHandler(tagRepository, linkRepository, ArticleRepository);

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
        A.CallTo(() => linkRepository.CreateAsync(A<ArticleTagLink>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task LinkTag_WithNotNullLink_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var Article = ArticleFactory.CreateArticle(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        var expected = LinkFactory.CreateArticleTagLink(Article, tag);

        var command = new LinkTagCommand()
        {
            UserId = userId,
            TagId = tag.Id,
            ArticleId = Guid.NewGuid()
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

        A.CallTo(() => linkRepository.CreateAsync(A<ArticleTagLink>._, A<CancellationToken>._))
            .Returns(expected.Id);

        var handler = new LinkTagCommandHandler(tagRepository, linkRepository, ArticleRepository);

        var result = await handler.Handle(command, CancellationToken.None);

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

        A.CallTo(() => linkRepository.CreateAsync(A<ArticleTagLink>._, A<CancellationToken>._))
            .MustNotHaveHappened();

        result.Should().Be(expected.Id);
    }
}