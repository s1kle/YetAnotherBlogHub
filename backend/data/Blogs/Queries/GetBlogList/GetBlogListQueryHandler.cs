using AutoMapper;
using BlogHub.Data.Interfaces;
using BlogHub.Data.Queries.GetBlog;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Queries.GetBlogList;

public class GetBlogListQueryHandler : IRequestHandler<GetBlogListQuery, BlogListVm>
{
    private readonly IBlogDbContext _context;
    private readonly IMapper _mapper;

    public GetBlogListQueryHandler(IBlogDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BlogListVm> Handle(GetBlogListQuery request, CancellationToken cancellationToken)
    {
        var blogs = await _context.Blogs
            .Where(blog => blog.UserId.Equals(request.UserId))
            .Select(blog => _mapper.Map<BlogVmForList>(blog))
            .ToArrayAsync(cancellationToken);

        return new BlogListVm { Blogs = blogs };
    }
}