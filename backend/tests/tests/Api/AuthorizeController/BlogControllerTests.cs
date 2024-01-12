using BlogHub.Api.Controllers;
using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Blogs.Queries.ListSearch;
using BlogHub.Data.Blogs.Queries.ListSort;
using BlogHub.Data.Tags.Queries.Get;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api.AuthorizeController;

public class BlogControllerTests
{
    [Fact]
    public async void GetAll_Empty_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        var expected = new OkObjectResult(new BlogListVm() {
            Blogs = Array.Empty<BlogVmForList>()
        });

        var query = new GetListDto()
        {
            Page = 0,
            Size = 10
        };

        var temp = await fixture.Controller.GetAll(query);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void GetAll_NotEmpty_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        var size = 5;

        var blogs = BlogFactory.CreateBlogs(size, fixture.UserId);

        var expected = new OkObjectResult(new BlogListVm() {
            Blogs = blogs.Select(blog => new BlogVmForList()
            {
                UserId = fixture.UserId,
                Title = blog.Title,
                Id = blog.Id,
                CreationDate = blog.CreationDate,
                Tags = Array.Empty<TagVm>()
            }).ToList()
        });

        await fixture.AddRangeAsync(blogs);

        var query = new GetListDto()
        {
            Page = 0,
            Size = 10
        };

        var temp = await fixture.Controller.GetAll(query);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void GetAll_WithSearchFilter_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        var searchQuery = "searched";
        var searchProperties = "title";

        var size = 5;

        var searchedBlogs = BlogFactory.CreateBlogs(size, fixture.UserId, searchQuery);
        var blogs = BlogFactory.CreateBlogs(size, fixture.UserId);

        var expected = new OkObjectResult(new BlogListVm() {
            Blogs = searchedBlogs.Select(blog => new BlogVmForList()
            {
                UserId = fixture.UserId,
                Title = blog.Title,
                Id = blog.Id,
                CreationDate = blog.CreationDate,
                Tags = Array.Empty<TagVm>()
            }).ToList()
        });

        await fixture.AddRangeAsync(blogs);
        await fixture.AddRangeAsync(searchedBlogs);

        var queryDto = new GetListDto()
        {
            Page = 0,
            Size = 10
        };

        var searchDto = new ListSearchDto()
        {
            SearchQuery = searchQuery,
            SearchProperties = searchProperties
        };

        var temp = await fixture.Controller.GetAll(queryDto, searchDto: searchDto);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void GetAll_WithSortFilter_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        var sortProperty = "title";
        var sortDirection = "desc";

        var size = 5;

        var blogs = BlogFactory.CreateBlogs(size, fixture.UserId);

        var expected = new OkObjectResult(new BlogListVm() {
            Blogs = blogs.Select(blog => new BlogVmForList()
            {
                UserId = fixture.UserId,
                Title = blog.Title,
                Id = blog.Id,
                CreationDate = blog.CreationDate,
                Tags = Array.Empty<TagVm>()
            }).OrderByDescending(blog => blog.Title).ToList()
        });

        await fixture.AddRangeAsync(blogs);

        var queryDto = new GetListDto()
        {
            Page = 0,
            Size = 10
        };

        var sortDto = new ListSortDto()
        {
            SortProperty = sortProperty,
            SortDirection = sortDirection
        };

        var temp = await fixture.Controller.GetAll(queryDto, sortDto: sortDto);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void GetAll_NotEmptyWithExtraBlogs_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        var size = 5;

        var blogs = BlogFactory.CreateBlogs(size, fixture.UserId);
        var extraBlogs = BlogFactory.CreateBlogs(size, Guid.NewGuid());

        var expected = new OkObjectResult(new BlogListVm() {
            Blogs = blogs.Select(blog => new BlogVmForList()
            {
                UserId = fixture.UserId,
                Title = blog.Title,
                Id = blog.Id,
                CreationDate = blog.CreationDate,
                Tags = Array.Empty<TagVm>()
            }).ToList()
        });

        await fixture.AddRangeAsync(blogs);
        await fixture.AddRangeAsync(extraBlogs);

        var query = new GetListDto()
        {
            Page = 0,
            Size = 10
        };

        var temp = await fixture.Controller.GetAll(query);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void GetAll_WithInvalidQuery_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        var query = new GetListDto()
        {
            Page = 0,
            Size = 0
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.GetAll(query);
        });

        query = new GetListDto()
        {
            Page = -1,
            Size = 10
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.GetAll(query);
        });

        fixture.EnsureDeleted();
    }
}