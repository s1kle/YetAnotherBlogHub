namespace BlogHub.Data.Tags.Queries.GetList.Blog;

internal sealed class GetBlogTagListQueryValidator : AbstractValidator<GetBlogTagListQuery>
{
    public GetBlogTagListQueryValidator()
    {
        RuleFor(query => query.BlogId)
            .NotEmpty();
    }
}