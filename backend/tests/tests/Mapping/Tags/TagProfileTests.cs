using BlogHub.Data.Common.Mappings;
using BlogHub.Data.Tags.Get.Helpers;

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
}