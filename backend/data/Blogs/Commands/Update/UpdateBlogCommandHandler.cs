using AutoMapper;
using BlogHub.Data.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Commands.Update;

public class UpdateBlogCommandHandler : IRequestHandler<UpdateBlogCommand, Guid>
{
    private readonly IBlogDbContext _context;
    private readonly IMapper _mapper;

    public UpdateBlogCommandHandler(IBlogDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Blogs.FirstOrDefaultAsync(blog =>
            blog.Id == request.Id, cancellationToken);

        if (entity is null || entity.UserId != request.UserId)
            throw new ArgumentException(nameof(entity));

        var blog = entity with
        {
            Title = request.Title,
            Details = request.Details,

            EditDate = DateTime.UtcNow
        };

        _context.Blogs.Remove(entity);
        var result = await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync(cancellationToken);

        return blog.Id;
    }
}
