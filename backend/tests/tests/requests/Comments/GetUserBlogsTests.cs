using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Blogs.Queries.GetList.User;
using BlogHub.Data.Comments.Queries.GetList;
using BlogHub.Data.Comments.Queries.GetList.Blog;

namespace BlogHub.Tests.Requests.Comments;

public class GetBlogCommentsTests
{
    [Fact]
    public async Task GetCommentList_WithCorrectData_ShouldSuccess()
    {
        var size = 10;

        var blog = BlogFactory.CreateBlog(userId: Guid.NewGuid());

        var comments = CommentFactory.CreateComments(size, blog, blog.UserId);

        var expected = comments.Select(comment => new CommentVm()
        {
            UserId = comment.UserId,
            Id = comment.Id,
            BlogId = comment.BlogId,
            Content = comment.Content,
            CreationDate = blog.CreationDate
        }).ToList();

        var query = new GetBlogCommentListQuery()
        {
            BlogId = blog.Id
        };

        var repository = A.Fake<ICommentRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => repository.GetAllByBlogIdAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(comments);
            
        A.CallTo(() => mapper.Map<CommentVm>(A<Comment>._))
            .ReturnsNextFromSequence(expected.ToArray());

        var handler = new GetBlogCommentListQueryHandler(repository, mapper);

        var result = await handler.Handle(query, CancellationToken.None);

        A.CallTo(() => repository.GetAllByBlogIdAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid blogId, CancellationToken token) =>
            {
                blogId.Should().Be(blog.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => mapper.Map<CommentVm>(A<Comment>._))
            .WhenArgumentsMatch((object arg) =>
            {
                var actual = (Comment?)arg;
                comments.IndexOf(actual!).Should().NotBe(-1);
                return true;
            })
            .MustHaveHappened(size, Times.Exactly);

        var actual = result;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }
}