using System.Reflection;

namespace BlogHub.Tests.Validation.Comments;


public class BaseTests
{
    [Theory]
    [InlineData(typeof(CreateCommentCommand))]
    [InlineData(typeof(DeleteCommentCommand))]
    [InlineData(typeof(GetBlogCommentsQuery))]
    public void ValidRequests_ShouldSuccess(Type type)
    {
        var requestsMethod = typeof(ValidatorFactory)
            .GetMethod(nameof(ValidatorFactory.CreateValidRequest), BindingFlags.Static | BindingFlags.Public)!
            .MakeGenericMethod(type);

        var request = requestsMethod.Invoke(null, null)
            ?? throw new ArgumentNullException();

        var validatorsMethod = typeof(ValidatorFactory)
            .GetMethod(nameof(ValidatorFactory.GetValidators), BindingFlags.Static | BindingFlags.Public)!
            .MakeGenericMethod(type);

        var validators = (IEnumerable<IValidator>)(validatorsMethod.Invoke(null, null)
            ?? throw new ArgumentNullException());

        var contextMethod = typeof(ValidatorFactory)
            .GetMethod(nameof(ValidatorFactory.GetValidationContext), BindingFlags.Static | BindingFlags.Public)!
            .MakeGenericMethod(type);

        var context = (IValidationContext)(contextMethod.Invoke(null, new[] { request })
            ?? throw new ArgumentNullException());

        validators
            .Select(validator => validator.Validate(context))
            .All(result => result.IsValid)
            .Should().BeTrue();
    }
}