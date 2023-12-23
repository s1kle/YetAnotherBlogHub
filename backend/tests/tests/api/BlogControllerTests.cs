using BlogHub.Data.Commands.Create;
using BlogHub.Data.Commands.Update;
using BlogHub.Data.Queries.Get;
using BlogHub.Data.Queries.GetList;
using BlogHub.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api;

public class BlogControllerTests
{
    private readonly FixtureFactory _fixtureFactory;

    public BlogControllerTests()
    {
        _fixtureFactory = new ();
    }

    [Fact]
    public async Task GetAllFromEmptyList_WithValidParams_ShouldSuccess()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture();
        var blogController = blogControllerFixture.BlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var dto = new GetListDto() 
        {
            Page = 0,
            Size = 10
        };

        var result = (await blogController.GetAll(dto)).Result as OkObjectResult;
        var value = result.Value as BlogListVm;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        value.Should().NotBeNull();
        value.Blogs.Should().NotBeNull();
        value.Blogs.Should().BeEmpty();

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllFromEmptyList_WithInvalidParams_ShouldFail()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture();
        var blogController = blogControllerFixture.BlogController;

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
    public async Task GetAll_WithValidParams_ShouldSuccess()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture();
        var blogController = blogControllerFixture.BlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var count = 5;
        var expectedList = new List<BlogVmForList>();

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

            expectedList.Add(new ()
            {
                Id = id,
                Title = title
            });
        }

        await blogControllerFixture.BlogDbContext.SaveChangesAsync();

        var dto = new GetListDto()
        {
            Page = 0,
            Size = 10
        };

        var result = (await blogController.GetAll(dto)).Result as OkObjectResult;
        var value = result.Value as BlogListVm;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        value.Should().NotBeNull();
        value.Blogs.Should().NotBeNull();
        value.Blogs.Count.Should().Be(count);
        value.Blogs.Should().BeInAscendingOrder(blog => blog.Id);
        value.Blogs.Should().ContainInOrder(expectedList.OrderBy(blog => blog.Id));

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAll_WithInvalidParams_ShouldFail()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture();
        var blogController = blogControllerFixture.BlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var id = Guid.NewGuid();
        var title = "Title";
        var details = "Details";
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
        await blogControllerFixture.BlogDbContext.SaveChangesAsync();

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

    [Fact]
    public async Task Create_WithValidParams_ShouldSuccess()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture();
        var blogController = blogControllerFixture.BlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var count = 5;
        var expectedList = new List<Blog>();
        var dtoList = new List<CreateBlogDto>();

        for (var i = 0; i < count; i++)
        {
            var title = $"Title{i}";
            var details = $"Details{i}";

            var blog = new Blog()
            {
                Id = Guid.NewGuid(),
                UserId = blogControllerFixture.UserId,
                Title = title,
                Details = details,
                CreationDate = DateTime.UtcNow,
                EditDate = null
            };

            dtoList.Add(new ()
            {
                Title = title,
                Details = details
            });

            expectedList.Add(blog);
        }

        foreach (var command in dtoList)
        {
            var result = (await blogController.Create(command)).Result as OkObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        var createdBlogs =  blogControllerFixture.BlogDbContext.Blogs.ToList();

        createdBlogs.Count().Should().Be(5);

        for (var i = 0; i < count; i++)
        {
            createdBlogs[i].UserId.Should().Be(expectedList[i].UserId);
            createdBlogs[i].Title.Should().BeEquivalentTo(expectedList[i].Title);
            createdBlogs[i].Details.Should().BeEquivalentTo(expectedList[i].Details);
            createdBlogs[i].CreationDate.Should().BeOnOrAfter(expectedList[i].CreationDate);
            createdBlogs[i].EditDate.Should().BeNull();
        }

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task Create_WithInvalidParams_ShouldFail()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture();
        var blogController = blogControllerFixture.BlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var createBlogDto = new CreateBlogDto()
        {
            Title = new ('a', 101),
            Details = new ('a', 101)
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await blogController.Create(createBlogDto);
        });

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task Delete_WithValidParams_ShouldSuccess()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture();
        var blogController = blogControllerFixture.BlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var id = Guid.NewGuid();

        var blog = new Blog()
        {
            Id = id,
            UserId = blogControllerFixture.UserId,
            Title = "Title",
            Details = "Details",
            CreationDate = DateTime.UtcNow,
            EditDate = null
        };

        await blogControllerFixture.BlogDbContext.Blogs.AddAsync(blog);
        await blogControllerFixture.BlogDbContext.SaveChangesAsync();

        blogControllerFixture.BlogDbContext.Blogs.Count().Should().Be(1);

        var result = (await blogController.Delete(id)).Result as OkObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);

        blogControllerFixture.BlogDbContext.Blogs.Should().BeEmpty();

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }
    
    [Fact]
    public async Task Delete_WithInvalidParams_ShouldFail()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture();
        var blogController = blogControllerFixture.BlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var id = Guid.NewGuid();
        var userId = blogControllerFixture.UserId;

        var blog = new Blog()
        {
            Id = id,
            UserId = userId,
            Title = "Title",
            Details = "Details",
            CreationDate = DateTime.UtcNow,
            EditDate = null
        };

        await blogControllerFixture.BlogDbContext.Blogs.AddAsync(blog);
        await blogControllerFixture.BlogDbContext.SaveChangesAsync();

        blogControllerFixture.BlogDbContext.Blogs.Count().Should().Be(1);

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await blogController.Delete(Guid.Empty);
        });

        blogControllerFixture.ChangeUser(blogControllerFixture.WrongUserId);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            var result = await blogController.Delete(id);
        });

        blogControllerFixture.BlogDbContext.Blogs.Count().Should().Be(1);

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task Update_WithValidParams_ShouldSuccess()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture();
        var blogController = blogControllerFixture.BlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var id = Guid.NewGuid();

        var date = DateTime.UtcNow;

        var blog = new Blog()
        {
            Id = id,
            UserId = blogControllerFixture.UserId,
            Title = "Title",
            Details = "Details",
            CreationDate = date,
            EditDate = null
        };

        var newTitle = "new Title";
        var newDetails = "new Details";

        var dto = new UpdateBlogDto()
        {
            Title = newTitle,
            Details = newDetails
        };

        await blogControllerFixture.BlogDbContext.Blogs.AddAsync(blog);
        await blogControllerFixture.BlogDbContext.SaveChangesAsync();

        blogControllerFixture.BlogDbContext.Blogs.Count().Should().Be(1);

        var result = (await blogController.Update(id, dto)).Result as OkObjectResult;
        await blogControllerFixture.BlogDbContext.Entry(blog).ReloadAsync();

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);

        blogControllerFixture.BlogDbContext.Blogs.Count().Should().Be(1);

        var editedBlog = blogControllerFixture.BlogDbContext.Blogs.FirstOrDefault();
        
        editedBlog.UserId.Should().Be(blogControllerFixture.UserId);
        editedBlog.Id.Should().Be(id);
        editedBlog.Title.Should().BeEquivalentTo(newTitle);
        editedBlog.Details.Should().BeEquivalentTo(newDetails);
        editedBlog.CreationDate.Should().BeOnOrAfter(date);
        editedBlog.EditDate.Should().NotBeBefore(date);

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task Update_WithInvalidParams_ShouldFail()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture();
        var blogController = blogControllerFixture.BlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var wrongId = Guid.NewGuid();
        var id = Guid.NewGuid();

        var blog = new Blog()
        {
            Id = id,
            UserId = blogControllerFixture.UserId,
            Title = "Title",
            Details = "Details",
            CreationDate = DateTime.UtcNow,
            EditDate = null
        };

        await blogControllerFixture.BlogDbContext.Blogs.AddAsync(blog);
        await blogControllerFixture.BlogDbContext.SaveChangesAsync();

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await blogController
                .Update(Guid.Empty, new () { Title = "T", Details = "D"});
        });

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await blogController
                .Update(id, new () { Title = new ('a', 101), Details = "D"});
        });

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            var result = await blogController
                .Update(wrongId, new () { Title = "T", Details = "D"});
        });

        blogControllerFixture.ChangeUser(blogControllerFixture.WrongUserId);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            var result = await blogController
                .Update(id, new () { Title = "T", Details = "D"});
        });

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task Get_WithValidParams_ShouldSuccess()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture();
        var blogController = blogControllerFixture.BlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var id = Guid.NewGuid();

        var date = DateTime.UtcNow;

        var blog = new Blog()
        {
            Id = id,
            UserId = blogControllerFixture.UserId,
            Title = "Title",
            Details = "Details",
            CreationDate = date,
            EditDate = null
        };

        await blogControllerFixture.BlogDbContext.Blogs.AddAsync(blog);
        await blogControllerFixture.BlogDbContext.SaveChangesAsync();

        blogControllerFixture.BlogDbContext.Blogs.Count().Should().Be(1);

        var result = (await blogController.GetById(id)).Result as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);

        blogControllerFixture.BlogDbContext.Blogs.Count().Should().Be(1);

        var value = result.Value as BlogVm;
        
        value.Id.Should().Be(blog.Id);
        value.Title.Should().BeEquivalentTo(blog.Title);
        value.Details.Should().BeEquivalentTo(blog.Details);
        value.CreationDate.Should().BeSameDateAs(blog.CreationDate);
        value.EditDate.Should().BeNull();

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task Get_WithInvalidParams_ShouldFail()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture();
        var blogController = blogControllerFixture.BlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var id = Guid.NewGuid();
        var wrongId = Guid.NewGuid();

        var blog = new Blog()
        {
            Id = id,
            UserId = blogControllerFixture.UserId,
            Title = "Title",
            Details = "Details",
            CreationDate = DateTime.UtcNow,
            EditDate = null
        };

        await blogControllerFixture.BlogDbContext.Blogs.AddAsync(blog);
        await blogControllerFixture.BlogDbContext.SaveChangesAsync();

        blogControllerFixture.BlogDbContext.Blogs.Count().Should().Be(1);

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await blogController.GetById(Guid.Empty);
        });

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            var result = await blogController.GetById(wrongId);
        });

        blogControllerFixture.ChangeUser(blogControllerFixture.WrongUserId);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            var result = await blogController.GetById(id);
        });

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }
}