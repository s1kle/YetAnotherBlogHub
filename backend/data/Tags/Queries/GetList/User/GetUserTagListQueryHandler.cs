using AutoMapper;
using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Tags.Queries.GetList;

public class GetUserTagListQueryHandler : IRequestHandler<GetUserTagListQuery, TagListVm>
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;

    public GetUserTagListQueryHandler(ITagRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<TagListVm> Handle(GetUserTagListQuery request, CancellationToken cancellationToken)
    {
        var tags = await _repository.GetAllByUserIdAsync(request.UserId, cancellationToken);

        if (tags is null) return new () { Tags = Array.Empty<TagVmForList>() };

        var mappedTags = tags.Select(_mapper.Map<TagVmForList>).ToList();

        return new () { Tags = mappedTags };
    }
}