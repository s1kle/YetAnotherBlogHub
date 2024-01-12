using BlogHub.Data.Tags.Queries.Get;

namespace BlogHub.Data.Tags.Queries.GetList;

internal sealed class GetTagListQueryHandler : IRequestHandler<GetTagListQuery, IReadOnlyList<TagVm>>
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;

    public GetTagListQueryHandler(ITagRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<IReadOnlyList<TagVm>> Handle(GetTagListQuery request, CancellationToken cancellationToken)
    {
        var tags = await _repository.GetAllAsync(cancellationToken);

        if (tags is null) return Array.Empty<TagVm>();

        return tags.Select(_mapper.Map<TagVm>).ToList();
    }
}