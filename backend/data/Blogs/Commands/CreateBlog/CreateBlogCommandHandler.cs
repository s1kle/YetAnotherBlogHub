using BlogHub.Data.Interfaces;
using BlogHub.Domain;
using MediatR;

namespace BlogHub.Data.Commands.CreateBlog;

public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, Guid>
{
    private readonly IBlogDbContext _context;

    public CreateBlogCommandHandler(IBlogDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = new Blog()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title,
            Details = request.Details,

            CreationDate = DateTime.UtcNow,
            EditDate = null
        };

        await _context.Blogs.AddAsync(blog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return blog.Id;
    }
}