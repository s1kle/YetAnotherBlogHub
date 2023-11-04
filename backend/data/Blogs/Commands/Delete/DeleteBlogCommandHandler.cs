using BlogHub.Data.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Commands.Delete;

public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommand, Guid>
{
    private readonly IBlogDbContext _context;

    public DeleteBlogCommandHandler(IBlogDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Blogs.FirstOrDefaultAsync(blog =>
            blog.Id == request.Id, cancellationToken);

        if (entity is null || entity.UserId != request.UserId)
            throw new ArgumentException(nameof(entity));

        _context.Blogs.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}