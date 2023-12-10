using AutoMapper;
using BlogHub.Data.Interfaces;
using BlogHub.Data.Queries.Get;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Queries.GetList;

public class GetBlogListQueryHandler : IRequestHandler<GetBlogListQuery, BlogListVm>
{
    private readonly IBlogRepository _repository;
    private readonly IMapper _mapper;

    public GetBlogListQueryHandler(IBlogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BlogListVm> Handle(GetBlogListQuery request, CancellationToken cancellationToken)
    {
        var blogs = _repository
            .GetAllBlogsAsync(request.UserId, request.Page, request.Size);

        var mappedBlogs = await blogs
            .Select(blog => _mapper.Map<BlogVmForList>(blog))
            .ToArrayAsync(cancellationToken);

        return new BlogListVm { Blogs = mappedBlogs };
    }
}