namespace BlogHub.Data.Tags.List.Blog;

internal sealed class Handler : IRequestHandler<Query, IReadOnlyList<TagVm>>
{
    private readonly TagsContext.Repository _tagRepository;
    private readonly BlogTagsContext.Repository _linkRepository;
    private readonly IMapper _mapper;

    public Handler(TagsContext.Repository tagRepository, BlogTagsContext.Repository linkRepository, IMapper mapper) =>
        (_tagRepository, _linkRepository, _mapper) = (tagRepository, linkRepository, mapper);

    public async Task<IReadOnlyList<TagVm>> Handle(Query request, CancellationToken cancellationToken)
    {
        var links = await _linkRepository.GetAllByBlogIdAsync(request.BlogId, cancellationToken);

        if (links is null) return Array.Empty<TagVm>();

        var tags = new List<Tag>();

        foreach(var link in links)
        {
            var tag = await _tagRepository.GetAsync(link.TagId, cancellationToken);

            if (tag is null) continue;

            tags.Add(tag);
        }

        return tags.Select(_mapper.Map<TagVm>).ToList();
    }
}