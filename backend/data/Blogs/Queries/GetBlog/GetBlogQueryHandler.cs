using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Queries.GetBlog;

public class GetBlogQueryHandler : IRequestHandler<GetBlogQuery, BlogVm>
{
    private readonly IBlogDbContext _context;
    private readonly IMapper _mapper;

    public GetBlogQueryHandler(IBlogDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BlogVm> Handle(GetBlogQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Blogs.FirstOrDefaultAsync(blog =>
            blog.Id == request.Id, cancellationToken);

        if (entity is null || entity.UserId != request.UserId)
            throw new ArgumentException(nameof(entity));

        return _mapper.Map<BlogVm>(entity);
    }
}