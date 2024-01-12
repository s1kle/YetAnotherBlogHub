namespace BlogHub.Data.Blogs.Queries.GetList.All;

internal sealed class GetBlogListQueryValidator : AbstractValidator<GetBlogListQuery>
{
    public GetBlogListQueryValidator()
    {
        RuleFor(query => query.Size)
            .GreaterThan(0);

        RuleFor(query => query.Page)
            .GreaterThan(-1);
    }
}