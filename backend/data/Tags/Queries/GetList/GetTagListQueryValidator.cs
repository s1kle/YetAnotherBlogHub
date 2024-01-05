using FluentValidation;

namespace BlogHub.Data.Tags.Queries.GetList;

public class GetTagListQueryValidator : AbstractValidator<GetTagListQuery>
{
    public GetTagListQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEmpty();
    }
}