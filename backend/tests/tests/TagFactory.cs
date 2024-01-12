namespace BlogHub.Tests;

public class TagFactory
{
    public static Tag CreateTag(string name = "name", Guid? userId = null) => new Tag()
    {
        Id = Guid.NewGuid(),
        UserId = userId ?? Guid.NewGuid(),
        Name = name
    };

    public static List<Tag> CreateTags(int size, Guid? userId = null) => Enumerable
        .Range(0, size)
        .Select(index => CreateTag($"Name {index}", userId))
        .ToList();
}