namespace BlogHub.Data.Pipeline;

public sealed class PipelineBuilder<Context>
{
    private readonly List<IPipelineStep<Context>> _steps;

    public PipelineBuilder()
    {
        _steps = new ();
    }

    public PipelineBuilder<Context> Add(IPipelineStep<Context> step)
    {
        _steps.Add(step);
        return this;
    }

    public Func<Context, Task<Context>> Build()
    {
        _steps.Reverse();

        return _steps.Aggregate(async (Context context) => await Task.FromResult(context),
             (next, current) => async (Context context) => await current.ExecuteAsync(context, next));
    }
}