namespace CrossCutting.ProcessingPipeline;

public interface IPipelineFeature<TModel> : IBuilderSource<IPipelineFeature<TModel>>
{
    Result<TModel> Process(PipelineContext<TModel> context);
}

public interface IPipelineFeature<TModel, TContext> : IBuilderSource<IPipelineFeature<TModel, TContext>>
{
    Result<TModel> Process(PipelineContext<TModel, TContext> context);
}
