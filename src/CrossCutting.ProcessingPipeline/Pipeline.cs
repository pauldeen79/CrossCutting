namespace CrossCutting.ProcessingPipeline;

public class Pipeline<TModel> : PipelineBase<TModel>
{
    public Pipeline(IEnumerable<IPipelineFeature<TModel>> features) : base(features)
    {
        ArgumentGuard.IsNotNull(features, nameof(features));
    }

    public void Process(TModel model)
    {
        var pipelineContext = new PipelineContext<TModel>(ArgumentGuard.IsNotNull(model, nameof(model)));

        foreach (var feature in Features)
        {
            feature.Process(pipelineContext);
        }
    }
}

public class Pipeline<TModel, TContext> : PipelineBase<TModel, TContext>
{
    public Pipeline(IEnumerable<IPipelineFeature<TModel, TContext>> features) : base(features)
    {
        ArgumentGuard.IsNotNull(features, nameof(features));
    }

    public void Process(TModel model, TContext context)
    {
        var pipelineContext = new PipelineContext<TModel, TContext>(model.IsNotNull(nameof(model)), context.IsNotNull(nameof(context)));

        foreach (var feature in Features)
        {
            feature.Process(pipelineContext);
        }
    }
}
