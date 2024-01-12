namespace BlogHub.Data.Pipeline.Helpers;

public interface IStep<Context>
{
    Task<Context> ExecuteAsync(Context context, Func<Context, Task<Context>> next);
}