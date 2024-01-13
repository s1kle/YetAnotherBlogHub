namespace BlogHub.Data.Tags.Unlink;

internal sealed class UnlinkTagCommandHandler : IRequestHandler<UnlinkTagCommand, Guid>
{
    private readonly TagRepository _tagRepository;
    private readonly BlogTagRepository _linkRepository;
    private readonly BlogRepository _blogRepository;

    public UnlinkTagCommandHandler(TagRepository tagRepository,
        BlogTagRepository linkRepository,
        BlogRepository blogRepository)
    {
        _tagRepository = tagRepository;
        _linkRepository = linkRepository;
        _blogRepository = blogRepository;
    }

    public async Task<Guid> Handle(UnlinkTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetAsync(request.TagId, cancellationToken);

        if (tag is null) throw new NotFoundException(nameof(tag));

        var blog = await _blogRepository.GetAsync(request.BlogId, cancellationToken);

        if (blog is null || blog.UserId != request.UserId) throw new NotFoundException(nameof(blog));

        var link = await _linkRepository.GetAsync(blog.Id, tag.Id, cancellationToken);

        if (link is null) throw new NotFoundException(nameof(blog));

        var id = await _linkRepository.RemoveAsync(link, cancellationToken);

        return id;
    }
}