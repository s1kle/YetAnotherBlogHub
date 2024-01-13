using BlogHub.Data.Blogs.Get.Helpers;
using BlogHub.Data.Blogs.List.Helpers;
using BlogHub.Data.Common.Mappings;

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
    public void BlogListItemVmMapping_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var blog = BlogFactory.CreateBlog(userId: userId);
        var expected = new BlogListItemVm()
        {
            UserId = userId,
            Id = blog.Id,
            Title = blog.Title,
            CreationDate = blog.CreationDate
        };

        var mapper = new MapperConfiguration(config => config.AddProfile<BlogMappingProfile>())
            .CreateMapper();

        var actual = mapper.Map<BlogListItemVm>(blog);

        actual.Should().BeEquivalentTo(expected);
    }
}