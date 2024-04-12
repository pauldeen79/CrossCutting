namespace CrossCutting.ProcessingPipeline;

public interface IPipelineComponent<TModel>
{
    Task<Result<TModel>> Process(PipelineContext<TModel> context, CancellationToken token);
}

public interface IPipelineComponent<TModel, TContext>
{
    Task<Result<TModel>> Process(PipelineContext<TModel, TContext> context, CancellationToken token);
}
