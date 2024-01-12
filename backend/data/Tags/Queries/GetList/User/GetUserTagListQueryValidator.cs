namespace BlogHub.Data.Tags.Queries.GetList;

internal sealed class GetUserTagListQueryValidator : AbstractValidator<GetUserTagListQuery>
{
    public GetUserTagListQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEmpty();
    }
}