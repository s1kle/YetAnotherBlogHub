namespace BlogHub.Data.Tags.List.User;

internal sealed class GetUserTagsQueryValidator : AbstractValidator<GetUserTagsQuery>
{
    public GetUserTagsQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEmpty();
    }
}