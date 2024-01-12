namespace BlogHub.Data.Tags.Queries.GetList.User;

internal sealed class GetUserTagListQueryValidator : AbstractValidator<GetUserTagListQuery>
{
    public GetUserTagListQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEmpty();
    }
}