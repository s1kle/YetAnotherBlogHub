using FluentValidation;

namespace BlogHub.Data.Tags.Queries.GetList.ByUserId;

public class GetTagListByUserIdQueryValidator : AbstractValidator<GetTagListByUserIdQuery>
{
    public GetTagListByUserIdQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEqual(Guid.Empty);
    }
}