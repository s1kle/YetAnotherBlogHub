namespace BlogHub.Data.Pipeline.Helpers;

public sealed record PipelineMethod<Context>
{
    public required Func<Context, Task<Context>> Func { get; init; }

    public Task<Context> ExecuteAsync(Context context) => Func(context);
}