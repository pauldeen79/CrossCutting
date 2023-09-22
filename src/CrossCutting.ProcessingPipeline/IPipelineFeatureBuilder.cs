namespace CrossCutting.ProcessingPipeline;

public interface IPipelineFeatureBuilder<TModel> : IBuilder<IPipelineFeature<TModel>>
{
}

public interface IPipelineFeatureBuilder<TModel, TContext> : IBuilder<IPipelineFeature<TModel, TContext>>
{
}
