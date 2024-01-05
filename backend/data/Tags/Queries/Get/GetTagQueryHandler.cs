using AutoMapper;
using BlogHub.Data.Exceptions;
using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Tags.Queries.Get;

public class GetTagQueryHandler : IRequestHandler<GetTagQuery, TagVm>
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;

    public GetTagQueryHandler(ITagRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<TagVm> Handle(GetTagQuery request, CancellationToken cancellationToken)
    {
        var tag = await _repository.GetAsync(request.Id, cancellationToken);

        if (tag is null) throw new NotFoundException(nameof(tag));

        return _mapper.Map<TagVm>(tag);
    }
}