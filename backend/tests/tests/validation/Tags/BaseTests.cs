using System.Reflection;
using BlogHub.Data.Tags.Commands.Create;
using BlogHub.Data.Tags.Commands.Delete;
using BlogHub.Data.Tags.Commands.Link;
using BlogHub.Data.Tags.Commands.Unlink;
using BlogHub.Data.Tags.Queries.Get;
using BlogHub.Data.Tags.Queries.GetList;

namespace BlogHub.Tests.Validation.Tags;


public class BaseTests
{
    [Theory]
    [InlineData(typeof(CreateTagCommand))]
    [InlineData(typeof(DeleteTagCommand))]
    [InlineData(typeof(LinkTagCommand))]
    [InlineData(typeof(UnlinkTagCommand))]
    [InlineData(typeof(GetTagQuery))]
    [InlineData(typeof(GetTagListQuery))]
    [InlineData(typeof(GetBlogTagListQuery))]
    [InlineData(typeof(GetUserTagListQuery))]
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

        var context = (IValidationContext)(contextMethod.Invoke(null, new [] { request }) 
            ?? throw new ArgumentNullException());

        validators
            .Select(validator => validator.Validate(context))
            .All(result => result.IsValid)
            .Should().BeTrue();
    }
}