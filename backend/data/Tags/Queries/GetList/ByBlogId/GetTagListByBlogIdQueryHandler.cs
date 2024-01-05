using AutoMapper;
using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Tags.Queries.GetList.ByBlogId;

public class GetTagListByBlogIdQueryHandler : IRequestHandler<GetTagListByBlogIdQuery, TagListVm>
{
    private readonly ITagRepository _tagRepository;
    private readonly IBlogTagRepository _blogTagRepository;
    private readonly IMapper _mapper;

    public GetTagListByBlogIdQueryHandler(ITagRepository tagRepository, IMapper mapper, IBlogTagRepository blogTagRepository) =>
        (_tagRepository, _mapper, _blogTagRepository) = (tagRepository, mapper, blogTagRepository);

    public async Task<TagListVm> Handle(GetTagListByBlogIdQuery request, CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.GetAllAsync(cancellationToken);

        if (tags is null) return new TagListVm { Tags = Array.Empty<TagVmForList>() };

        var blogTags = await _blogTagRepository.GetAllByBlogIdAsync(request.BlogId, cancellationToken);

        if (blogTags is null) return new TagListVm { Tags = Array.Empty<TagVmForList>() };

        var mappedTags = tags
            .Where(tag => blogTags.Any(blogTag => blogTag.TagId.Equals(tag.Id)))
            .Select(tag => _mapper.Map<TagVmForList>(tag))
            .ToList();

        return new TagListVm { Tags = mappedTags };
    }
}