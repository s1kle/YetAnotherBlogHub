using BlogHub.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Tests.Api.AuthorizeController.Articles;

public class DeleteCommentTests
{
    [Fact]
    public async Task DeleteComment_WithValidData_ShouldDeleteComment()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var Article = ArticleFactory.CreateArticle("123", fixture.UserId);
        var comment = CommentFactory.CreateComment(Article, fixture.UserId);

        await fixture.AddRangeAsync(new[] { Article });
        await fixture.AddRangeAsync(new[] { comment });

        var temp = await fixture.Controller.DeleteComment(Article.Id, comment.Id);

        var result = temp.Result as OkObjectResult;

        result.Should().NotBeNull();

        var deletedComment = await fixture.CommentDbContext.Comments.FirstOrDefaultAsync();

        deletedComment.Should().BeNull();

        fixture.EnsureDeleted();
    }

    [Fact]
    public async Task DeleteComment_WithNonExistentComment_ShouldReturnNotFound()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await fixture.Controller.DeleteComment(Guid.NewGuid(), Guid.NewGuid());
        });

        fixture.EnsureDeleted();
    }

    [Fact]
    public async Task DeleteComment_WithInvalidCommentId_ShouldReturnNotFound()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.DeleteComment(Guid.NewGuid(), Guid.Empty);
        });
    }
}