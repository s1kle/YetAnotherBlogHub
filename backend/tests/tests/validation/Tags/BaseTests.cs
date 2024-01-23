using System.Reflection;

namespace BlogHub.Tests.Validation.Tags;


public class BaseTests
{
    [Theory]
    [InlineData(typeof(CreateTagCommand))]
    [InlineData(typeof(DeleteTagCommand))]
    [InlineData(typeof(LinkTagCommand))]
    [InlineData(typeof(UnlinkTagCommand))]
    [InlineData(typeof(GetAllTagsQuery))]
    [InlineData(typeof(GetArticleTagsQuery))]
    [InlineData(typeof(GetUserTagsQuery))]
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