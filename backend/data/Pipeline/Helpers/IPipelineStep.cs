namespace BlogHub.Data.Pipeline.Helpers;

public interface IPipelineStep<Context>
{
    Task<Context> ExecuteAsync(Context context, Func<Context, Task<Context>> next);
}