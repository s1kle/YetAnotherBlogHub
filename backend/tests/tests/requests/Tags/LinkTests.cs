using BlogHub.Data.Exceptions;
using BlogHub.Data.Interfaces;
using BlogHub.Data.Tags.Commands.Link;
using BlogHub.Domain;

namespace BlogHub.Tests.Requests.Tags;

public class LinkTests
{
    [Fact]
    public async Task LinkTag_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var blog = BlogFactory.CreateBlog(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        BlogTagLink? temp = null;
        var expected = LinkFactory.CreateBlogTagLink(blog, tag);

        var command = new LinkTagCommand()
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
            .Returns(temp);

        A.CallTo(() => linkRepository.CreateAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .Returns(expected.Id);
    
        var handler = new LinkTagCommandHandler(tagRepository, linkRepository, blogRepository);
    
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

        A.CallTo(() => linkRepository.CreateAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .WhenArgumentsMatch((BlogTagLink actual, CancellationToken token) =>
            {
                actual.BlogId.Should().Be(expected.BlogId);
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
        var blog = BlogFactory.CreateBlog(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        BlogTagLink? temp = null;
        var expected = LinkFactory.CreateBlogTagLink(blog, tag);

        var command = new LinkTagCommand()
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

        A.CallTo(() => linkRepository.CreateAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .Returns(expected.Id);
    
        var handler = new LinkTagCommandHandler(tagRepository, linkRepository, blogRepository);
        
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
        A.CallTo(() => linkRepository.CreateAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task LinkTag_WithIncorrectBlogId_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var blog = BlogFactory.CreateBlog(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        BlogTagLink? temp = null;
        var expected = LinkFactory.CreateBlogTagLink(blog, tag);

        var command = new LinkTagCommand()
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

        A.CallTo(() => linkRepository.CreateAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .Returns(expected.Id);
    
        var handler = new LinkTagCommandHandler(tagRepository, linkRepository, blogRepository);
        
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
        A.CallTo(() => linkRepository.CreateAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task LinkTag_WithNotNullLink_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var blog = BlogFactory.CreateBlog(userId: userId);
        var tag = TagFactory.CreateTag(userId: userId);
        var expected = LinkFactory.CreateBlogTagLink(blog, tag);

        var command = new LinkTagCommand()
        {
            UserId = userId,
            TagId = tag.Id,
            BlogId = Guid.NewGuid()
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

        A.CallTo(() => linkRepository.CreateAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .Returns(expected.Id);
    
        var handler = new LinkTagCommandHandler(tagRepository, linkRepository, blogRepository);
        
        var result = await handler.Handle(command, CancellationToken.None);

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

        A.CallTo(() => linkRepository.CreateAsync(A<BlogTagLink>._, A<CancellationToken>._))
            .MustNotHaveHappened();

        result.Should().Be(expected.Id);
    }
}