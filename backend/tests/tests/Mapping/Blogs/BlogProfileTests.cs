using BlogHub.Data.Blogs.Queries.Get;
using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Mappings;

namespace BlogHub.Tests.Mapping.Blogs;

public class BlogProfileTests
{
    [Fact]
    public void BlogVmMapping_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var blog = BlogFactory.CreateBlog(userId: userId);
        var expected = new BlogVm()
        {
            UserId = userId,
            Id = blog.Id,
            Title = blog.Title,
            CreationDate = blog.CreationDate,
            EditDate = blog.EditDate,
            Details = blog.Details,
        };
        
        var mapper = new MapperConfiguration(config => config.AddProfile<BlogMappingProfile>())
            .CreateMapper();

        var actual = mapper.Map<BlogVm>(blog);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void BlogVmForListMapping_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var blog = BlogFactory.CreateBlog(userId: userId);
        var expected = new BlogVmForList()
        {
            UserId = userId,
            Id = blog.Id,
            Title = blog.Title,
            CreationDate = blog.CreationDate
        };
        
        var mapper = new MapperConfiguration(config => config.AddProfile<BlogMappingProfile>())
            .CreateMapper();

        var actual = mapper.Map<BlogVmForList>(blog);

        actual.Should().BeEquivalentTo(expected);
    }
}