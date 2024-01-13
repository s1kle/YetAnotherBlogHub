namespace BlogHub.Data.Tags.List.All;

internal sealed class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, IReadOnlyList<TagVm>>
{
    private readonly TagRepository _repository;
    private readonly IMapper _mapper;

    public GetAllTagsQueryHandler(TagRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<IReadOnlyList<TagVm>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _repository.GetAllAsync(cancellationToken);

        if (tags is null) return Array.Empty<TagVm>();

        return tags.Select(_mapper.Map<TagVm>).ToList();
    }
}