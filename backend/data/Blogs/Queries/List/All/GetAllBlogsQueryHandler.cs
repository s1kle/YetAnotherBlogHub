namespace BlogHub.Data.Blogs.List.All;

internal sealed class GetAllBlogsQueryHandler : IRequestHandler<GetAllBlogsQuery, BlogListVm>
{
    private readonly BlogRepository _repository;
    private readonly IMapper _mapper;

    public GetAllBlogsQueryHandler(BlogRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<BlogListVm> Handle(GetAllBlogsQuery request, CancellationToken cancellationToken)
    {
        var blogs = await _repository
            .GetAllAsync(request.Page, request.Size, cancellationToken);

        if (blogs is null) return new() { Blogs = Array.Empty<BlogListItemVm>() };

        return new() { Blogs = blogs.Select(_mapper.Map<BlogListItemVm>).ToList() };
    }
}