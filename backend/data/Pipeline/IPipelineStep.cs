namespace BlogHub.Data.Pipeline;

public interface IPipelineStep<Context>
{
    Task<Context> ExecuteAsync(Context context, Func<Context, Task<Context>> next);
}