namespace CrossCutting.ProcessingPipeline;

public class PipelineContext<TModel, TContext>
{
    public PipelineContext(TModel model, TContext context)
    {
        Model = ArgumentGuard.IsNotNull(model, nameof(model));
        Context = ArgumentGuard.IsNotNull(context, nameof(context));
    }

    public TModel Model { get; }
    public TContext Context { get; }
}
