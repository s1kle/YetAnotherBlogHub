namespace BlogHub.Data.Blogs.List.User;

internal sealed class GetUserBlogsQueryValidator : AbstractValidator<GetUserBlogsQuery>
{
    public GetUserBlogsQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEqual(Guid.Empty);

        RuleFor(query => query.Size)
            .GreaterThan(0);

        RuleFor(query => query.Page)
            .GreaterThan(-1);
    }
}