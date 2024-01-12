namespace BlogHub.Data.Common.Mappings;

internal sealed class BlogMappingProfile : Profile
{
    public BlogMappingProfile()
    {
        CreateMap<Blog, BlogVm>();
        CreateMap<Blog, ItemVm>();
    }
}