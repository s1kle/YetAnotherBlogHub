using BlogHub.Data.Interfaces;
using BlogHub.Domain;
using MediatR;

namespace BlogHub.Data.BlogTags.Queries.GetList;

public class GetBlogTagListQueryHandler : IRequestHandler<GetBlogTagListQuery, BlogTagListVm>
{
    private readonly IBlogTagRepository _repository;

    public GetBlogTagListQueryHandler(IBlogTagRepository repository) =>
        _repository = repository;

    public async Task<BlogTagListVm> Handle(GetBlogTagListQuery request, CancellationToken cancellationToken)
    {
        var blogTags = await _repository
            .GetAllAsync(request.BlogId, cancellationToken);

        if (blogTags is null) return new BlogTagListVm { BlogTags = Array.Empty<BlogTag>() };


        return new BlogTagListVm { BlogTags = blogTags };
    }
}