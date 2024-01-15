using BlogHub.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api.AuthorizeController.Tags;

public class DeleteTests
{
    [Fact]
    public async void Delete_WithValidData_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeTagController>();
        fixture.EnsureCreated();

        var tag = TagFactory.CreateTag("1234567890", fixture.UserId);

        await fixture.AddRangeAsync(new[] { tag });

        var expected = new OkObjectResult(tag.Id);

        var temp = await fixture.Controller.Delete(tag.Id);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.TagDbContext.Tags.Should().BeEmpty();

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void Delete_WithEmptyArticleId_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeTagController>();
        fixture.EnsureCreated();

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.Delete(Guid.Empty);
        });

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void Delete_WithInvalidTagId_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeTagController>();
        fixture.EnsureCreated();


        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await fixture.Controller.Delete(Guid.NewGuid());
        });

        fixture.EnsureDeleted();
    }
}