using BlogHub.Data.Commands.Create;
using BlogHub.Data.Commands.Delete;
using BlogHub.Data.Commands.Update;
using BlogHub.Data.Queries.Get;
using BlogHub.Data.Queries.GetList;
using BlogHub.Tests.Fixtures.Validation;

namespace BlogHub.Tests.Data.Validation;

public class ValidationTests
{
    private readonly ValidationFixture _validationFixture;

    public ValidationTests()
    {
        var fixtureFactory = new FixtureFactory();
        _validationFixture = fixtureFactory.ValidationFixture();
    }

    #region DeleteBlogCommand
    [Fact]
    public void ValidDeleteBlogCommand_ShouldSuccess()
    {
        var requestType = typeof(DeleteBlogCommand);
        var request = _validationFixture.GetValidDeleteBlogCommand();
        var validators = _validationFixture.GetValidators(requestType);
        var context = new ValidationContext<DeleteBlogCommand>(request);
        
        validators
            .Select(validator => validator.Validate(context))
            .Count(validator => validator.Errors.Count > 0)
            .Should().Be(0);
    }

    [Fact]
    public void InvalidDeleteBlogCommand_ShouldFail()
    {
        var requestType = typeof(DeleteBlogCommand);
        var requests = _validationFixture.GetInvalidDeleteBlogCommand();
        var validators = _validationFixture.GetValidators(requestType);

        foreach(var context in requests
            .Select(request => new ValidationContext<DeleteBlogCommand>(request)))
        {
            validators
                .Select(validator => validator.Validate(context))
                .Count(validator => validator.Errors.Count > 0)
                .Should().NotBe(0);
        }
        
    }
    #endregion
    #region CreateBlogCommand
    [Fact]
    public void ValidCreateBlogCommand_ShouldSuccess()
    {
        var requestType = typeof(CreateBlogCommand);
        var request = _validationFixture.GetValidCreateBlogCommand();
        var validators = _validationFixture.GetValidators(requestType);
        var context = new ValidationContext<CreateBlogCommand>(request);
        
        validators
            .Select(validator => validator.Validate(context))
            .Count(validator => validator.Errors.Count > 0)
            .Should().Be(0);
    }

    [Fact]
    public void InvalidCreateBlogCommand_ShouldFail()
    {
        var requestType = typeof(CreateBlogCommand);
        var requests = _validationFixture.GetInvalidCreateBlogCommand();
        var validators = _validationFixture.GetValidators(requestType);

        foreach(var context in requests
            .Select(request => new ValidationContext<CreateBlogCommand>(request)))
        {
            validators
                .Select(validator => validator.Validate(context))
                .Count(validator => validator.Errors.Count > 0)
                .Should().NotBe(0);
        }
        
    }
    #endregion
    #region UpdateBlogCommand
    [Fact]
    public void ValidUpdateBlogCommand_ShouldSuccess()
    {
        var requestType = typeof(UpdateBlogCommand);
        var request = _validationFixture.GetValidUpdateBlogCommand();
        var validators = _validationFixture.GetValidators(requestType);
        var context = new ValidationContext<UpdateBlogCommand>(request);
        
        validators
            .Select(validator => validator.Validate(context))
            .Count(validator => validator.Errors.Count > 0)
            .Should().Be(0);
    }

    [Fact]
    public void InvalidUpdateBlogCommand_ShouldFail()
    {
        var requestType = typeof(UpdateBlogCommand);
        var requests = _validationFixture.GetInvalidUpdateBlogCommand();
        var validators = _validationFixture.GetValidators(requestType);

        foreach(var context in requests
            .Select(request => new ValidationContext<UpdateBlogCommand>(request)))
        {
            validators
                .Select(validator => validator.Validate(context))
                .Count(validator => validator.Errors.Count > 0)
                .Should().NotBe(0);
        }
        
    }
    #endregion
    #region GetBlogQuery
    [Fact]
    public void ValidGetBlogQuery_ShouldSuccess()
    {
        var requestType = typeof(GetBlogQuery);
        var request = _validationFixture.GetValidGetBlogQuery();
        var validators = _validationFixture.GetValidators(requestType);
        var context = new ValidationContext<GetBlogQuery>(request);
        
        validators
            .Select(validator => validator.Validate(context))
            .Count(validator => validator.Errors.Count > 0)
            .Should().Be(0);
    }

    [Fact]
    public void InvalidGetBlogQuery_ShouldFail()
    {
        var requestType = typeof(GetBlogQuery);
        var requests = _validationFixture.GetInvalidGetBlogQuery();
        var validators = _validationFixture.GetValidators(requestType);

        foreach(var context in requests
            .Select(request => new ValidationContext<GetBlogQuery>(request)))
        {
            validators
                .Select(validator => validator.Validate(context))
                .Count(validator => validator.Errors.Count > 0)
                .Should().NotBe(0);
        }
        
    }
    #endregion
    #region GetBlogListQuery
    [Fact]
    public void ValidGetBlogListQuery_ShouldSuccess()
    {
        var requestType = typeof(GetBlogListQuery);
        var request = _validationFixture.GetValidGetBlogListQuery();
        var validators = _validationFixture.GetValidators(requestType);
        var context = new ValidationContext<GetBlogListQuery>(request);
        
        validators
            .Select(validator => validator.Validate(context))
            .Count(validator => validator.Errors.Count > 0)
            .Should().Be(0);
    }

    [Fact]
    public void InvalidGetBlogListQuery_ShouldFail()
    {
        var requestType = typeof(GetBlogListQuery);
        var requests = _validationFixture.GetInvalidGetBlogListQuery();
        var validators = _validationFixture.GetValidators(requestType);

        foreach(var context in requests
            .Select(request => new ValidationContext<GetBlogListQuery>(request)))
        {
            validators
                .Select(validator => validator.Validate(context))
                .Count(validator => validator.Errors.Count > 0)
                .Should().NotBe(0);
        }
        
    }
    #endregion
}