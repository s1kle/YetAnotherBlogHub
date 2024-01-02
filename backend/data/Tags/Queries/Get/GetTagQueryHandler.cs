using AutoMapper;
using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Tags.Queries.Get;

public class GetTagQueryHandler : IRequestHandler<GetTagQuery, TagVm>
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;

    public GetTagQueryHandler(ITagRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TagVm> Handle(GetTagQuery request, CancellationToken cancellationToken)
    {
        var tag = await _repository.GetAsync(request.Id, cancellationToken);

        if (tag is null || tag.BlogId != request.BlogId)
            throw new ArgumentException(nameof(tag));

        return _mapper.Map<TagVm>(tag);
    }
}