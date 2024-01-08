using BlogHub.Data.Interfaces;
using BlogHub.Data.Tags.Commands.Create;
using BlogHub.Domain;

namespace BlogHub.Tests.Requests.Tags;

public class CreateTests
{
    [Fact]
    public async Task CreateTag_ShouldSuccess()
    {
        var expected = TagFactory.CreateTag("name");

        var command = new CreateTagCommand()
        {
            UserId = expected.UserId,
            Name = expected.Name
        };

        var repository = A.Fake<ITagRepository>();
    
        A.CallTo(() => repository.CreateAsync(A<Tag>._, A<CancellationToken>._))
            .Returns(expected.Id);

        var handler = new CreateTagCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => repository.CreateAsync(A<Tag>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Tag actual, CancellationToken token) =>
            {
                actual.UserId.Should().Be(expected.UserId);
                actual.Name.Should().BeEquivalentTo(expected.Name);

                token.Should().Be(CancellationToken.None);

                return true;
            })
            .MustHaveHappenedOnceExactly();
    }
}