using AutoMapper;
using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Tags.Queries.GetList.ByUserId;

public class GetTagListByUserIdQueryHandler : IRequestHandler<GetTagListByUserIdQuery, TagListVm>
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;

    public GetTagListByUserIdQueryHandler(ITagRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<TagListVm> Handle(GetTagListByUserIdQuery request, CancellationToken cancellationToken)
    {
        var tags = await (request.UserId is null
            ? _repository.GetAllAsync(cancellationToken)
            : _repository.GetAllByUserIdAsync(request.UserId.Value, cancellationToken));

        if (tags is null) return new TagListVm { Tags = Array.Empty<TagVmForList>() };

        var mappedTags = tags
            .Select(tag => _mapper.Map<TagVmForList>(tag))
            .ToList();

        return new TagListVm { Tags = mappedTags };
    }
}