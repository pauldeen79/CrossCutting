namespace CrossCutting.ProcessingPipeline;

public static class PipelineComponentExtensions
{
    public static Task<Result<TModel>> Process<TModel>(this IPipelineComponent<TModel> instance, PipelineContext<TModel> context)
        => instance.Process(context, CancellationToken.None);

    public static Task<Result<TModel>> Process<TModel, TContext>(this IPipelineComponent<TModel, TContext> instance, PipelineContext<TModel, TContext> context)
        => instance.Process(context, CancellationToken.None);
}
