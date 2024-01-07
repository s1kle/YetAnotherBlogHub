using AutoMapper;
using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Tags.Queries.GetList;

public class GetTagListQueryHandler : IRequestHandler<GetTagListQuery, TagListVm>
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;

    public GetTagListQueryHandler(ITagRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<TagListVm> Handle(GetTagListQuery request, CancellationToken cancellationToken)
    {
        var tags = await _repository.GetAllAsync(cancellationToken);

        if (tags is null) return new () { Tags = Array.Empty<TagVmForList>() };

        var mappedTags = tags.Select(_mapper.Map<TagVmForList>).ToList();

        return new () { Tags = mappedTags };
    }
}