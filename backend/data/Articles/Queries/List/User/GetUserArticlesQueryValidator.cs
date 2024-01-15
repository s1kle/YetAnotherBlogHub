namespace BlogHub.Data.Articles.List.User;

internal sealed class GetUserArticlesQueryValidator : AbstractValidator<GetUserArticlesQuery>
{
    public GetUserArticlesQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEqual(Guid.Empty);

        RuleFor(query => query.Size)
            .GreaterThan(0);

        RuleFor(query => query.Page)
            .GreaterThan(-1);
    }
}