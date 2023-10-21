namespace CrossCutting.ProcessingPipeline;

public interface IPipelineFeature<TModel> : IBuilderSource<IPipelineFeature<TModel>>
{
    Result Process(PipelineContext<TModel> context);
}

public interface IPipelineFeature<TModel, TContext> : IBuilderSource<IPipelineFeature<TModel, TContext>>
{
    Result<TContext> Process(PipelineContext<TModel, TContext> context);
}
