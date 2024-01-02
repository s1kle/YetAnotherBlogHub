using BlogHub.Data.Commands.Create;
using BlogHub.Data.Commands.Update;
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

    [Fact]
    public void Mapping_CreateBlogDtoToCreateBlogCommand_ShouldSuccess()
    {
        var fixture = _blogsFactory.MappingCreateDtoFixture("Title", "Details");
        var userId = fixture.UserId;
        var expected = fixture.Command;
        var dto = fixture.Dto;

        var result = _mapper.Map<CreateBlogCommand>(dto);

        var actual = result with { UserId = userId };

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Mapping_UpdateBlogDtoToUpdateBlogCommand_ShouldSuccess()
    {
        var fixture = _blogsFactory.MappingUpdateDtoFixture("Title", "Details");
        var id = fixture.Id;
        var userId = fixture.UserId;
        var expected = fixture.Command;
        var dto = fixture.Dto;

        var result = _mapper.Map<UpdateBlogCommand>(dto);

        var actual = result with { UserId = userId, Id = id };

        actual.Should().BeEquivalentTo(expected);
    }
}