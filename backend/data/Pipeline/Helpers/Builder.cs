namespace BlogHub.Data.Pipeline.Helpers;

public sealed class Builder<Context>
{
    private readonly List<IStep<Context>> _steps;

    public Builder() =>
        _steps = new ();

    public Builder<Context> Add(IStep<Context> step)
    {
        _steps.Add(step);
        return this;
    }

    public Method<Context> Build()
    {
        _steps.Reverse();

        return new () { Func =
            _steps.Aggregate(
                async (Context context) => await Task.FromResult(context),
                (next, current) => async (Context context) => await current.ExecuteAsync(context, next)) };
    }
}