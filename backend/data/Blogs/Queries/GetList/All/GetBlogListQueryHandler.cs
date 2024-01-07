using AutoMapper;
using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Blogs.Queries.GetList;

public class GetBlogListQueryHandler : IRequestHandler<GetBlogListQuery, BlogListVm>
{
    private readonly IBlogRepository _repository;
    private readonly IMapper _mapper;

    public GetBlogListQueryHandler(IBlogRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<BlogListVm> Handle(GetBlogListQuery request, CancellationToken cancellationToken)
    {
        var blogs = await _repository
            .GetAllAsync(request.Page, request.Size, cancellationToken);

        if (blogs is null) return new BlogListVm { Blogs = new List<BlogVmForList>() };

        var mappedBlogs = blogs
            .Select(_mapper.Map<BlogVmForList>)
            .ToList();

        return new BlogListVm { Blogs = mappedBlogs };
    }
}