using AutoMapper;
using BlogHub.Data.Commands.CreateBlog;
using BlogHub.Data.Queries.GetBlog;
using BlogHub.Data.Queries.GetBlogList;
using BlogHub.Domain;

namespace BlogHub.Data.Mappings;

public class BlogMappingProfile : Profile
{
    public BlogMappingProfile()
    {
        CreateMap<Blog, BlogVm>();
        CreateMap<Blog, BlogVmForList>();
        CreateMap<CreateBlogModel, CreateBlogCommand>();
    }
}