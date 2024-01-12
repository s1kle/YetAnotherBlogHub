namespace BlogHub.Data.Tags.Queries.GetList;

internal sealed class GetBlogTagListQueryValidator : AbstractValidator<GetBlogTagListQuery>
{
    public GetBlogTagListQueryValidator()
    {
        RuleFor(query => query.BlogId)
            .NotEmpty();
    }
}