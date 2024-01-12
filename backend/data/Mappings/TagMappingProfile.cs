using BlogHub.Data.Tags.Queries.Get;

namespace BlogHub.Data.Mappings;

internal sealed class TagMappingProfile : Profile
{
    public TagMappingProfile()
    {
        CreateMap<Tag, TagVm>();
    }
}