namespace BlogHub.Data.Tags.Unlink;

internal sealed class Handler : IRequestHandler<Command, Guid>
{
    private readonly TagsContext.Repository _tagRepository;
    private readonly BlogTagsContext.Repository _linkRepository;
    private readonly BlogsContext.Repository _blogRepository;

    public Handler(TagsContext.Repository tagRepository,
        BlogTagsContext.Repository linkRepository,
        BlogsContext.Repository blogRepository)
    {
        _tagRepository = tagRepository;
        _linkRepository = linkRepository;
        _blogRepository = blogRepository;
    }

    public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
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