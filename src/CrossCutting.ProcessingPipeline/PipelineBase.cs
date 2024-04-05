namespace CrossCutting.ProcessingPipeline;

public class PipelineBase<TModel> : AbstractPipelineBase<IPipelineComponent<TModel>>
{
    public PipelineBase(IEnumerable<IPipelineComponent<TModel>>? features) : base(features)
    {
    }
}

public class PipelineBase<TModel, TContext> : AbstractPipelineBase<IPipelineComponent<TModel, TContext>>
{
    public PipelineBase(IEnumerable<IPipelineComponent<TModel, TContext>>? features) : base(features)
    {
    }
}
