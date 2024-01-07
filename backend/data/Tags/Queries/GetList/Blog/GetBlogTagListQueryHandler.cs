using AutoMapper;
using BlogHub.Data.Interfaces;
using BlogHub.Domain;
using MediatR;

namespace BlogHub.Data.Tags.Queries.GetList;

public class GetBlogTagListQueryHandler : IRequestHandler<GetBlogTagListQuery, TagListVm>
{
    private readonly ITagRepository _tagRepository;
    private readonly IBlogTagRepository _linkRepository;
    private readonly IMapper _mapper;

    public GetBlogTagListQueryHandler(ITagRepository tagRepository, IBlogTagRepository linkRepository, IMapper mapper) =>
        (_tagRepository, _linkRepository, _mapper) = (tagRepository, linkRepository, mapper);

    public async Task<TagListVm> Handle(GetBlogTagListQuery request, CancellationToken cancellationToken)
    {
        var links = await _linkRepository.GetAllByBlogIdAsync(request.BlogId, cancellationToken);

        if (links is null) return new () { Tags = Array.Empty<TagVmForList>() };

        var tags = new List<Tag>();

        foreach(var link in links)
        {
            var tag = await _tagRepository.GetAsync(link.TagId, cancellationToken);

            if (tag is null) continue;

            tags.Add(tag);
        }

        var mappedTags = tags.Select(_mapper.Map<TagVmForList>).ToList();

        return new () { Tags = mappedTags };
    }
}