namespace CrossCutting.ProcessingPipeline;

public class PipelineContext<TModel>
{
    public PipelineContext(TModel model)
    {
        Model = model.IsNotNull(nameof(model));
    }

    public TModel Model { get; }
}

public class PipelineContext<TModel, TContext> : PipelineContext<TModel>
{
    public PipelineContext(TModel model, TContext context) : base(model)
    {
        Context = context.IsNotNull(nameof(context));
    }

    public TContext Context { get; }
}
