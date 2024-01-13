namespace BlogHub.Data.Tags.List.User;

internal sealed class GetUserTagsQueryHandler : IRequestHandler<GetUserTagsQuery, IReadOnlyList<TagVm>>
{
    private readonly TagRepository _repository;
    private readonly IMapper _mapper;

    public GetUserTagsQueryHandler(TagRepository tagRepository, IMapper mapper) =>
        (_repository, _mapper) = (tagRepository, mapper);

    public async Task<IReadOnlyList<TagVm>> Handle(GetUserTagsQuery request, CancellationToken cancellationToken)
    {

        var tags = await _repository.GetAllByUserIdAsync(request.UserId, cancellationToken);

        if (tags is null) return Array.Empty<TagVm>();

        return tags.Select(_mapper.Map<TagVm>).ToList();
    }
}