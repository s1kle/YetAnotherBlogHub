namespace BlogHub.Data.Tags.Get.Helpers;

public sealed record TagVm
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}