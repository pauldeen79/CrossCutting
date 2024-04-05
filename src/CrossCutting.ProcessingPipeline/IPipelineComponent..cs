namespace CrossCutting.ProcessingPipeline;

public interface IPipelineComponent<TModel>
{
    Result<TModel> Process(PipelineContext<TModel> context);
}

public interface IPipelineComponent<TModel, TContext>
{
    Result<TModel> Process(PipelineContext<TModel, TContext> context);
}
