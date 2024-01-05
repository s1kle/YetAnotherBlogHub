using BlogHub.Data.Mappings;
using BlogHub.Data.Queries.Get;
using BlogHub.Data.Queries.GetList;

namespace BlogHub.Tests.Data.Mapping;

public class MappingTests
{
    private readonly IMapper _mapper;
    private readonly FixtureFactory _blogsFactory;
    public MappingTests()
    {
        _mapper = new MapperConfiguration(config => config.AddProfile<BlogMappingProfile>())
            .CreateMapper();
        _blogsFactory = new ();
    }

    [Fact]
    public void Mapping_BlogToBlogVm_ShouldSuccess()
    {
        var fixture = _blogsFactory.GetBlogFixture();
        var blog = fixture.Blog;
        var expected = fixture.BlogVm;

        var actual = _mapper.Map<BlogVm>(blog);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Mapping_BlogToBlogVmForList_ShouldSuccess()
    {
        var fixture = _blogsFactory.GetBlogListFixture(1);
        var blog = fixture.BlogList.First();
        var expected = fixture.BlogVmList.First();

        var actual = _mapper.Map<BlogVmForList>(blog);

        actual.Should().BeEquivalentTo(expected);
    }
}