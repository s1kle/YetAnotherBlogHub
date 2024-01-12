namespace BlogHub.Data.Tags.List.All;

internal sealed class Handler : IRequestHandler<Query, IReadOnlyList<TagVm>>
{
    private readonly TagsContext.Repository _repository;
    private readonly IMapper _mapper;

    public Handler(TagsContext.Repository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<IReadOnlyList<TagVm>> Handle(Query request, CancellationToken cancellationToken)
    {
        var tags = await _repository.GetAllAsync(cancellationToken);

        if (tags is null) return Array.Empty<TagVm>();

        return tags.Select(_mapper.Map<TagVm>).ToList();
    }
}