using BlogHub.Api.Controllers;
using BlogHub.Data.Tags.Get.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api.AuthorizeController.Tags;

public class GetAllTests
{
    [Fact]
    public async void GetAll_Empty_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeTagController>();
        fixture.EnsureCreated();

        var expected = new OkObjectResult(Array.Empty<TagVm>());

        var temp = await fixture.Controller.GetAll();

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void GetAll_NotEmpty_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeTagController>();
        fixture.EnsureCreated();

        var size = 5;

        var tags = TagFactory.CreateTags(size, fixture.UserId);

        var expected = new OkObjectResult(tags.Select(tag => new TagVm()
        {
            Name = tag.Name,
            Id = tag.Id
        }).ToList());

        await fixture.AddRangeAsync(tags);

        var temp = await fixture.Controller.GetAll();

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }
}