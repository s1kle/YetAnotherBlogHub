using BlogHub.Data.Queries.GetList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api.Methods;

public class GetAllByUserId_Tests
{
    private readonly FixtureFactory _fixtureFactory;
    private readonly string _dbContextName;

    public GetAllByUserId_Tests()
    {
        _fixtureFactory = new ();
        _dbContextName = $"{this.GetType()}";
    }

    #region ValidParams
    [Fact]
    public async Task GetAllFromEmptyList_WithValidParams_ShouldSuccess()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture(_dbContextName);
        var blogController = blogControllerFixture.AuthorizeBlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var dto = new GetListDto() 
        {
            Page = 0,
            Size = 10
        };

        var result = (await blogController.GetAll(dto)).Result as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var actual = result.Value as BlogListVm;

        actual.Should().NotBeNull();
        actual!.Blogs.Should().NotBeNull();
        actual.Blogs.Should().BeEmpty();

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAll_WithValidParams_ShouldSuccess()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture(_dbContextName);
        var blogController = blogControllerFixture.AuthorizeBlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var count = 5;
        var expected = new List<BlogVmForList>();

        for (var i = 0; i < count; i++)
        {
            var id = Guid.NewGuid();
            var title = $"Title{i}";
            var details = $"Details{i}";
            var creationDate = DateTime.UtcNow;
            DateTime? editDate = null;

            await blogControllerFixture.BlogDbContext.Blogs.AddAsync(new ()
            {
                Id = id,
                UserId = blogControllerFixture.UserId,
                Title = title,
                Details = details,
                CreationDate = creationDate,
                EditDate = editDate
            });

            expected.Add(new ()
            {
                Id = id,
                Title = title
            });
        }

        for (var i = 0; i < count; i++)
        {
            var id = Guid.NewGuid();
            var title = $"Title{i}";
            var details = $"Details{i}";
            var creationDate = DateTime.UtcNow;
            DateTime? editDate = null;

            await blogControllerFixture.BlogDbContext.Blogs.AddAsync(new ()
            {
                Id = id,
                UserId = blogControllerFixture.WrongUserId,
                Title = title,
                Details = details,
                CreationDate = creationDate,
                EditDate = editDate
            });
        }

        await blogControllerFixture.BlogDbContext.SaveChangesAsync();

        var dto = new GetListDto()
        {
            Page = 0,
            Size = 10
        };

        var result = (await blogController.GetAll(dto)).Result as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);
        
        var actual = result.Value as BlogListVm;

        actual.Should().NotBeNull();
        actual!.Blogs.Should().NotBeNull();
        actual.Blogs.Count.Should().Be(count);
        actual.Blogs.Should().Contain(expected);

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }
    #endregion

    #region InvalidParams
    [Fact]
    public async Task GetAllFromEmptyList_WithInvalidParams_ShouldFail()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture(_dbContextName);
        var blogController = blogControllerFixture.AuthorizeBlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var dto = new GetListDto() 
        {
            Page = 0,
            Size = 0
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await blogController.GetAll(dto);
        });

        dto = new ()
        {
            Page = -1,
            Size = 1
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await blogController.GetAll(dto);
        });

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAll_WithInvalidParams_ShouldFail()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture(_dbContextName);
        var blogController = blogControllerFixture.AuthorizeBlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var dto = new GetListDto()
        {
            Page = -1,
            Size = 10
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await blogController.GetAll(dto);
        });

        dto = new GetListDto()
        {
            Page = 0,
            Size = 0
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await blogController.GetAll(dto);
        });

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }
    #endregion
}