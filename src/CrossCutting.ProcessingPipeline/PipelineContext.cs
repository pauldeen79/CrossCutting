namespace CrossCutting.ProcessingPipeline;

public class PipelineContext<TModel>
{
    public PipelineContext(TModel model)
    {
        Model = ArgumentGuard.IsNotNull(model, nameof(model));
    }

    public TModel Model { get; }
}

public class PipelineContext<TModel, TContext> : PipelineContext<TModel>
{
    public PipelineContext(TModel model, TContext context) : base(model)
    {
        Context = ArgumentGuard.IsNotNull(context, nameof(context));
    }

    public TContext Context { get; }
}
