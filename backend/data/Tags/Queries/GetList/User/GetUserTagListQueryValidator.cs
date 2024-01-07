using FluentValidation;

namespace BlogHub.Data.Tags.Queries.GetList;

public class GetUserTagListQueryValidator : AbstractValidator<GetUserTagListQuery>
{
    public GetUserTagListQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEmpty();
    }
}