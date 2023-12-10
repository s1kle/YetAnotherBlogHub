using AutoMapper;
using BlogHub.Data.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Queries.Get;

public class GetBlogQueryHandler : IRequestHandler<GetBlogQuery, BlogVm>
{
    private readonly IBlogRepository _repository;
    private readonly IMapper _mapper;

    public GetBlogQueryHandler(IBlogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BlogVm> Handle(GetBlogQuery request, CancellationToken cancellationToken)
    {
        var blog = await _repository.GetBlogAsync(request.Id, cancellationToken);

        if (blog is null || blog.UserId != request.UserId)
            throw new ArgumentException(nameof(blog));

        return _mapper.Map<BlogVm>(blog);
    }
}