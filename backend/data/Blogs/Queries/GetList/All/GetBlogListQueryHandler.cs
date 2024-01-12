namespace BlogHub.Data.Blogs.Queries.GetList.All;

internal sealed class GetBlogListQueryHandler : IRequestHandler<GetBlogListQuery, BlogListVm>
{
    private readonly IBlogRepository _repository;
    private readonly IMapper _mapper;

    public GetBlogListQueryHandler(IBlogRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<BlogListVm> Handle(GetBlogListQuery request, CancellationToken cancellationToken)
    {
        var blogs = await _repository
            .GetAllAsync(request.Page, request.Size, cancellationToken);

        if (blogs is null) return new () { Blogs = Array.Empty<BlogVmForList>() };

        return new () { Blogs = blogs.Select(_mapper.Map<BlogVmForList>).ToList() };
    }
}