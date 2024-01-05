using AutoMapper;
using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Blogs.Queries.GetList;

public class GetBlogListQueryHandler : IRequestHandler<GetBlogListQuery, BlogListVm>
{
    private readonly IBlogRepository _repository;
    private readonly IMapper _mapper;
    private readonly ITagRepository _tagRepository;

    public GetBlogListQueryHandler(IBlogRepository repository, IMapper mapper, ITagRepository tagRepository) =>
        (_repository, _mapper, _tagRepository) = (repository, mapper, tagRepository);

    public async Task<BlogListVm> Handle(GetBlogListQuery request, CancellationToken cancellationToken)
    {
        var blogs = await (request.UserId is not null
            ? _repository.GetAllByUserIdAsync(request.UserId.Value,
                request.Page, request.Size, cancellationToken)
            : _repository.GetAllAsync(request.Page,
                request.Size, cancellationToken));

        if (blogs is null) return new BlogListVm { Blogs = new List<BlogVmForList>() };

        var mappedBlogs = blogs
            .Search(request.SearchFilter)
            .SortByProperty(request.SortFilter)
            .Select(blog => _mapper.Map<BlogVmForList>(blog))
            .ToList();

        return new BlogListVm { Blogs = mappedBlogs };
    }
}