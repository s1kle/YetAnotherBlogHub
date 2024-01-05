using AutoMapper;
using BlogHub.Data.Queries.Get;
using BlogHub.Data.Queries.GetList;
using BlogHub.Domain;

namespace BlogHub.Data.Mappings;

public class BlogMappingProfile : Profile
{
    public BlogMappingProfile()
    {
        CreateMap<Blog, BlogVm>();
        CreateMap<Blog, BlogVmForList>();
    }
}