using BlogHub.Api.Controllers;
using BlogHub.Data.Articles.Create.Helpers;
using BlogHub.Data.Tags.Create.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Tests.Api.AuthorizeController.Tags;

public class CreateTests
{
    [Fact]
    public async void Create_WithValidData_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeTagController>();
        fixture.EnsureCreated();

        var expected = TagFactory.CreateTag("12345", fixture.UserId);

        var dto = new CreateTagDto()
        {
            Name = expected.Name
        };

        var temp = await fixture.Controller.Create(dto);

        var result = temp.Result as OkObjectResult;

        result.Should().NotBeNull();

        var actual = await fixture.BlogHubDbContext.Tags.FirstOrDefaultAsync();

        actual.Should().NotBeNull();
        actual!.Name.Should().BeEquivalentTo(expected.Name);
        actual!.UserId.Should().Be(expected.UserId);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void Create_WithInvalidTitle_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeTagController>();
        fixture.EnsureCreated();

        var expected = TagFactory.CreateTag("12345678901", fixture.UserId);

        var dto = new CreateTagDto()
        {
            Name = expected.Name
        };


        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.Create(dto);
        });

        fixture.EnsureDeleted();
    }
}