namespace CrossCutting.ProcessingPipeline;

public class PipelineBase<TModel> : AbstractPipelineBase<IPipelineFeature<TModel>>
{
    public PipelineBase(IEnumerable<IPipelineFeature<TModel>> features) : base(features)
    {
    }
}

public class PipelineBase<TModel, TContext> : AbstractPipelineBase<IPipelineFeature<TModel, TContext>>
{
    public PipelineBase(IEnumerable<IPipelineFeature<TModel, TContext>> features) : base(features)
    {
    }
}
