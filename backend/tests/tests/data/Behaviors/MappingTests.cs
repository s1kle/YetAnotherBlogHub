using AutoMapper;
using BlogHub.Data.Mappings;

namespace BlogHub.Tests.Data.Behaviors;

public class MappingTests
{
    private readonly IMapper _mapper;
    private readonly BlogsFactory _blogsFactory;
    public MappingTests()
    {
        _mapper = new MapperConfiguration(config => config.AddProfile<BlogMappingProfile>())
            .CreateMapper();
        _blogsFactory = new ();
    }

    [Fact]
    public async Task _()
    {

    }
}