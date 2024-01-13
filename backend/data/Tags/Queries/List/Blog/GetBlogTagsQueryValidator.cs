namespace BlogHub.Data.Tags.List.Blog;

internal sealed class GetBlogTagsQueryValidator : AbstractValidator<GetBlogTagsQuery>
{
    public GetBlogTagsQueryValidator()
    {
        RuleFor(query => query.BlogId)
            .NotEmpty();
    }
}