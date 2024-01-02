using AutoMapper;
using BlogHub.Data.Blogs.Commands.Create;
using BlogHub.Data.Blogs.Commands.Update;
using BlogHub.Data.Blogs.Queries.Get;
using BlogHub.Data.Blogs.Queries.GetList;
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