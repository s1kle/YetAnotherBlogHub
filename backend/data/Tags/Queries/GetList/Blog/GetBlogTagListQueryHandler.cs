using BlogHub.Data.Tags.Queries.Get;

namespace BlogHub.Data.Tags.Queries.GetList.Blog;

internal sealed class GetBlogTagListQueryHandler : IRequestHandler<GetBlogTagListQuery, IReadOnlyList<TagVm>>
{
    private readonly ITagRepository _tagRepository;
    private readonly IBlogTagRepository _linkRepository;
    private readonly IMapper _mapper;

    public GetBlogTagListQueryHandler(ITagRepository tagRepository, IBlogTagRepository linkRepository, IMapper mapper) =>
        (_tagRepository, _linkRepository, _mapper) = (tagRepository, linkRepository, mapper);

    public async Task<IReadOnlyList<TagVm>> Handle(GetBlogTagListQuery request, CancellationToken cancellationToken)
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