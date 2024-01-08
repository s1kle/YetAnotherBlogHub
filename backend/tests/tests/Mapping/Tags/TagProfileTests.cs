using BlogHub.Data.Blogs.Queries.Get;
using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Mappings;
using BlogHub.Data.Tags.Queries.Get;
using BlogHub.Data.Tags.Queries.GetList;

namespace BlogHub.Tests.Mapping.Tags;

public class TagProfileTests
{
    [Fact]
    public void TagVmMapping_ShouldSuccess()
    {
        var tag = TagFactory.CreateTag();
        var expected = new TagVm()
        {
            Id = tag.Id,
            Name = tag.Name
        };
        
        var mapper = new MapperConfiguration(config => config.AddProfile<TagMappingProfile>())
            .CreateMapper();

        var actual = mapper.Map<TagVm>(tag);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void TagVmForListMapping_ShouldSuccess()
    {
        var tag = TagFactory.CreateTag();
        var expected = new TagVmForList()
        {
            Id = tag.Id,
            Name = tag.Name
        };
        
        var mapper = new MapperConfiguration(config => config.AddProfile<TagMappingProfile>())
            .CreateMapper();

        var actual = mapper.Map<TagVmForList>(tag);

        actual.Should().BeEquivalentTo(expected);
    }
}