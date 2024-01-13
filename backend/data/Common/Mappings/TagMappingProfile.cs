namespace BlogHub.Data.Common.Mappings;

internal sealed class TagMappingProfile : Profile
{
    public TagMappingProfile()
    {
        CreateMap<Tag, TagVm>();
    }
}