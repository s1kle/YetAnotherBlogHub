using AutoMapper;
using BlogHub.Data.Commands.Create;
using BlogHub.Data.Commands.Update;
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
        CreateMap<CreateBlogDto, CreateBlogCommand>();
        CreateMap<UpdateBlogDto, UpdateBlogCommand>();
    }
}