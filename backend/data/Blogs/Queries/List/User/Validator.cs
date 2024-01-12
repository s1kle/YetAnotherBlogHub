namespace BlogHub.Data.Blogs.List.User;

internal sealed class Validator : AbstractValidator<Query>
{
    public Validator()
    {
        RuleFor(query => query.UserId)
            .NotEqual(Guid.Empty);    

        RuleFor(query => query.Size)
            .GreaterThan(0);

        RuleFor(query => query.Page)
            .GreaterThan(-1);
    }
}