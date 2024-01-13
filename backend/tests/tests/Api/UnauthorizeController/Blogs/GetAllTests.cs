using BlogHub.Api.Controllers;
using BlogHub.Data.Blogs.List.Helpers;
using BlogHub.Data.Tags.Get.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api.UnauthorizeController.Blogs;

public class GetAllTests
{
    [Fact]
    public async void GetAll_Empty_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<UnauthorizeBlogController>();
        fixture.EnsureCreated();

        var expected = new OkObjectResult(new BlogListVm()
        {
            Blogs = Array.Empty<BlogListItemVm>()
        });

        var dto = new BlogListDto()
        {
            List = new() { Page = 0, Size = 10 }
        };

        var temp = await fixture.Controller.GetAll(dto);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void GetAll_NotEmpty_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<UnauthorizeBlogController>();
        fixture.EnsureCreated();

        var size = 5;

        var blogs = BlogFactory.CreateBlogs(size, fixture.UserId);

        var expected = new OkObjectResult(new BlogListVm()
        {
            Blogs = blogs.Select(blog => new BlogListItemVm()
            {
                UserId = fixture.UserId,
                Title = blog.Title,
                Id = blog.Id,
                CreationDate = blog.CreationDate,
                Tags = Array.Empty<TagVm>(),
            }).ToList()
        });

        await fixture.AddRangeAsync(blogs);

        var dto = new BlogListDto()
        {
            List = new() { Page = 0, Size = 10 }
        };

        var temp = await fixture.Controller.GetAll(dto);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void GetAll_WithSearchFilter_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<UnauthorizeBlogController>();
        fixture.EnsureCreated();

        var searchQuery = "searched";
        var searchProperties = "title";

        var size = 5;

        var searchedBlogs = BlogFactory.CreateBlogs(size, fixture.UserId, searchQuery);
        var blogs = BlogFactory.CreateBlogs(size, fixture.UserId);

        var expected = new OkObjectResult(new BlogListVm()
        {
            Blogs = searchedBlogs.Select(blog => new BlogListItemVm()
            {
                UserId = fixture.UserId,
                Title = blog.Title,
                Id = blog.Id,
                CreationDate = blog.CreationDate,
                Tags = Array.Empty<TagVm>(),
            }).ToList()
        });

        await fixture.AddRangeAsync(blogs);
        await fixture.AddRangeAsync(searchedBlogs);

        var dto = new BlogListDto()
        {
            List = new() { Page = 0, Size = 10 },
            Search = new()
            {
                Query = searchQuery,
                Properties = searchProperties
            }
        };

        var temp = await fixture.Controller.GetAll(dto);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void GetAll_WithSortFilter_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<UnauthorizeBlogController>();
        fixture.EnsureCreated();

        var sortProperty = "title";
        var sortDirection = "desc";

        var size = 5;

        var blogs = BlogFactory.CreateBlogs(size, fixture.UserId);

        var expected = new OkObjectResult(new BlogListVm()
        {
            Blogs = blogs.Select(blog => new BlogListItemVm()
            {
                UserId = fixture.UserId,
                Title = blog.Title,
                Id = blog.Id,
                CreationDate = blog.CreationDate,
                Tags = Array.Empty<TagVm>(),
            }).OrderByDescending(blog => blog.Title).ToList()
        });

        await fixture.AddRangeAsync(blogs);

        var dto = new BlogListDto()
        {
            List = new() { Page = 0, Size = 10 },
            Sort = new()
            {
                Property = sortProperty,
                Direction = sortDirection
            }
        };

        var temp = await fixture.Controller.GetAll(dto);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void GetAll_WithInvalidQuery_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<UnauthorizeBlogController>();
        fixture.EnsureCreated();

        var dto = new BlogListDto()
        {
            List = new() { Page = 0, Size = 0 }
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.GetAll(dto);
        });

        dto = new BlogListDto()
        {
            List = new() { Page = -1, Size = 10 }
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.GetAll(dto);
        });

        fixture.EnsureDeleted();
    }
}