namespace BlogHub.Data.Blogs.List.All;

internal sealed class GetAllBlogsQueryValidator : AbstractValidator<GetAllBlogsQuery>
{
    public GetAllBlogsQueryValidator()
    {
        RuleFor(query => query.Size)
            .GreaterThan(0);

        RuleFor(query => query.Page)
            .GreaterThan(-1);
    }
}