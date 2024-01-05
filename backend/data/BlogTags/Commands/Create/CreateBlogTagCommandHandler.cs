using BlogHub.Data.Exceptions;
using BlogHub.Data.Interfaces;
using BlogHub.Domain;
using MediatR;

namespace BlogHub.Data.BlogTags.Commands.Create;

public class CreateBlogTagCommandHandler : IRequestHandler<CreateBlogTagCommand, Guid>
{
    private readonly IBlogTagRepository _blogTagRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IBlogRepository _blogRepository;

    public CreateBlogTagCommandHandler(
        IBlogTagRepository blogTagRepository,
        ITagRepository tagRepository,
        IBlogRepository blogRepository) 
    {
        _blogTagRepository = blogTagRepository;
        _tagRepository = tagRepository;
        _blogRepository = blogRepository;
    }

    public async Task<Guid> Handle(CreateBlogTagCommand request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.GetAsync(request.BlogId, cancellationToken);

        if (blog is null || blog.UserId != request.UserId) throw new NotFoundException(nameof(blog));

        var tag = await _tagRepository.GetAsync(request.TagId, cancellationToken);

        if (tag is null) throw new NotFoundException(nameof(tag));

        var blogTag = new BlogTag()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            TagId = request.TagId,
            BlogId = request.BlogId
        };

        var id = await _blogTagRepository.CreateAsync(blogTag, cancellationToken);

        return id;
    }
}