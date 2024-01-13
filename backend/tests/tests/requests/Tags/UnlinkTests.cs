namespace BlogHub.Tests.Requests.Tags;

public class UninkTests
{
    [Fact]
    public async Task UnlinkTag_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var blog = BlogFactory.CreateBlog(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        var expected = LinkFactory.CreateBlogTagLink(blog, tag);

        var command = new UnlinkTagCommand()
        {
            UserId = userId,
            TagId = tag.Id,
            BlogId = blog.Id
        };

        var tagRepository = A.Fake<ITagRepository>();
        var linkRepository = A.Fake<IBlogTagRepository>();
        var blogRepository = A.Fake<IBlogRepository>();

        A.CallTo(() => blogRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(blog);
        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(tag);
        A.CallTo(() => linkRepository.GetAsync(A<Guid>._, A<Guid>._, A<CancellationToken>._))
            .Returns(expected);
        A.CallTo(() => linkRepository.RemoveAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .Returns(expected.Id);
    
        var handler = new UnlinkTagCommandHandler(tagRepository, linkRepository, blogRepository);
    
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
            .WhenArgumentsMatch((Guid blogId, Guid tagId, CancellationToken token) =>
            {
                tagId.Should().Be(tag.Id);
                blogId.Should().Be(blog.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => blogRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(blog.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => linkRepository.RemoveAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .WhenArgumentsMatch((BlogTagLink actual, CancellationToken token) =>
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
        var blog = BlogFactory.CreateBlog(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        BlogTagLink? temp = null;
        var expected = LinkFactory.CreateBlogTagLink(blog, tag);

        var command = new UnlinkTagCommand()
        {
            UserId = userId,
            TagId = Guid.NewGuid(),
            BlogId = blog.Id
        };

        tag = null;

        var tagRepository = A.Fake<ITagRepository>();
        var linkRepository = A.Fake<IBlogTagRepository>();
        var blogRepository = A.Fake<IBlogRepository>();

        A.CallTo(() => blogRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(blog);
        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(tag);
        A.CallTo(() => linkRepository.GetAsync(A<Guid>._, A<Guid>._, A<CancellationToken>._))
            .Returns(temp);

        A.CallTo(() => linkRepository.RemoveAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .Returns(expected.Id);
    
        var handler = new UnlinkTagCommandHandler(tagRepository, linkRepository, blogRepository);
        
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
        A.CallTo(() => blogRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .MustNotHaveHappened();
        A.CallTo(() => linkRepository.RemoveAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UnlinkTag_WithIncorrectBlogId_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var blog = BlogFactory.CreateBlog(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        BlogTagLink? temp = null;
        var expected = LinkFactory.CreateBlogTagLink(blog, tag);

        var command = new UnlinkTagCommand()
        {
            UserId = userId,
            TagId = tag.Id,
            BlogId = Guid.NewGuid()
        };

        blog = null;

        var tagRepository = A.Fake<ITagRepository>();
        var linkRepository = A.Fake<IBlogTagRepository>();
        var blogRepository = A.Fake<IBlogRepository>();

        A.CallTo(() => blogRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(blog);
        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(tag);
        A.CallTo(() => linkRepository.GetAsync(A<Guid>._, A<Guid>._, A<CancellationToken>._))
            .Returns(temp);

        A.CallTo(() => linkRepository.RemoveAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .Returns(expected.Id);
    
        var handler = new UnlinkTagCommandHandler(tagRepository, linkRepository, blogRepository);
        
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

        A.CallTo(() => blogRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.BlogId);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => linkRepository.GetAsync(A<Guid>._, A<Guid>._, A<CancellationToken>._))
            .MustNotHaveHappened();
        A.CallTo(() => linkRepository.RemoveAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UnlinkTag_WithIncorrectLinkId_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var blog = BlogFactory.CreateBlog(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        var expected = LinkFactory.CreateBlogTagLink(blog, tag);

        var command = new UnlinkTagCommand()
        {
            UserId = userId,
            TagId = tag.Id,
            BlogId = Guid.NewGuid()
        };

        expected = null;

        var tagRepository = A.Fake<ITagRepository>();
        var linkRepository = A.Fake<IBlogTagRepository>();
        var blogRepository = A.Fake<IBlogRepository>();

        A.CallTo(() => blogRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(blog);
        A.CallTo(() => tagRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(tag);
        A.CallTo(() => linkRepository.GetAsync(A<Guid>._, A<Guid>._, A<CancellationToken>._))
            .Returns(expected);
        A.CallTo(() => linkRepository.RemoveAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .Returns(Guid.Empty);
    
        var handler = new UnlinkTagCommandHandler(tagRepository, linkRepository, blogRepository);
        
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

        A.CallTo(() => blogRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.BlogId);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => linkRepository.GetAsync(A<Guid>._, A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid blogId, Guid tagId, CancellationToken token) =>
            {
                tagId.Should().Be(tag.Id);
                blogId.Should().Be(blog.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => linkRepository.RemoveAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }
}