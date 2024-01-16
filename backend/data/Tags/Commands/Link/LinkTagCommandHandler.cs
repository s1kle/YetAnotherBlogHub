namespace BlogHub.Data.Tags.Link;

internal sealed class LinkTagCommandHandler : IRequestHandler<LinkTagCommand, Guid>
{
    private readonly ITagRepository _tagRepository;
    private readonly IArticleTagRepository _linkRepository;
    private readonly IArticleRepository _ArticleRepository;

    public LinkTagCommandHandler(ITagRepository tagRepository,
        IArticleTagRepository linkRepository,
        IArticleRepository ArticleRepository)
    {
        _tagRepository = tagRepository;
        _linkRepository = linkRepository;
        _ArticleRepository = ArticleRepository;
    }

    public async Task<Guid> Handle(LinkTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetAsync(request.TagId, cancellationToken);

        if (tag is null) throw new NotFoundException(nameof(tag));

        var Article = await _ArticleRepository.GetAsync(request.ArticleId, cancellationToken);

        if (Article is null || Article.UserId != request.UserId) throw new NotFoundException(nameof(Article));

        var link = await _linkRepository.GetAsync(Article.Id, tag.Id, cancellationToken);

        if (link is not null) return link.Id;

        link = new ArticleTag()
        {
            Id = Guid.NewGuid(),
            ArticleId = Article.Id,
            TagId = tag.Id
        };

        var id = await _linkRepository.CreateAsync(link, cancellationToken);

        return id;
    }
}