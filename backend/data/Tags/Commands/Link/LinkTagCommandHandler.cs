namespace BlogHub.Data.Tags.Commands.Link;

internal sealed class LinkTagCommandHandler : IRequestHandler<LinkTagCommand, Guid>
{
    private readonly ITagRepository _tagRepository;
    private readonly IBlogTagRepository _linkRepository;
    private readonly IBlogRepository _blogRepository;

    public LinkTagCommandHandler(ITagRepository tagRepository,
                                 IBlogTagRepository linkRepository,
                                 IBlogRepository blogRepository)
    {
        _tagRepository = tagRepository;
        _linkRepository = linkRepository;
        _blogRepository = blogRepository;
    }

    public async Task<Guid> Handle(LinkTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetAsync(request.TagId, cancellationToken);

        if (tag is null) throw new NotFoundException(nameof(tag));

        var blog = await _blogRepository.GetAsync(request.BlogId, cancellationToken);

        if (blog is null || blog.UserId != request.UserId) throw new NotFoundException(nameof(blog));

        var link = await _linkRepository.GetAsync(blog.Id, tag.Id, cancellationToken);

        if (link is not null) return link.Id;

        link = new BlogTagLink()
        {
            Id = Guid.NewGuid(),
            BlogId = blog.Id,
            TagId = tag.Id
        };

        var id = await _linkRepository.CreateAsync(link, cancellationToken);

        return id;
    }
}