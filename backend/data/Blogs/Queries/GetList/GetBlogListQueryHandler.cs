using AutoMapper;
using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Blogs.Queries.GetList;

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
        var blogs = await _repository
            .GetAllAsync(request.UserId, request.Page, request.Size, cancellationToken)
            ?? new ();

        var mappedBlogs = blogs
            .Select(blog => _mapper.Map<BlogVmForList>(blog))
            .ToList();

        return new BlogListVm { Blogs = mappedBlogs };
    }
}