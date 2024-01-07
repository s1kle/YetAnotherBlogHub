using AutoMapper;
using BlogHub.Data.Tags.Queries.Get;
using BlogHub.Data.Tags.Queries.GetList;
using BlogHub.Domain;

namespace BlogHub.Data.Mappings;

public class TagMappingProfile : Profile
{
    public TagMappingProfile()
    {
        CreateMap<Tag, TagVm>();
        CreateMap<Tag, TagVmForList>();
    }
}