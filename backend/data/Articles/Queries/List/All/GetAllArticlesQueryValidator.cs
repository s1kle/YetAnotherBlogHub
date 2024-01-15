namespace BlogHub.Data.Articles.List.All;

internal sealed class GetAllArticlesQueryValidator : AbstractValidator<GetAllArticlesQuery>
{
    public GetAllArticlesQueryValidator()
    {
        RuleFor(query => query.Size)
            .GreaterThan(0);

        RuleFor(query => query.Page)
            .GreaterThan(-1);
    }
}