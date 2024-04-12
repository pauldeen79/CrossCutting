namespace CrossCutting.ProcessingPipeline;

public static class PipelineExtensions
{
    public static Task<Result<TModel>> Process<TModel>(this IPipeline<TModel> pipeline, TModel model)
        => pipeline.Process(model, new CancellationToken());

    public static Task<Result<TModel>> Process<TModel, TContext>(this IPipeline<TModel, TContext> pipeline, TModel model, TContext context)
        => pipeline.Process(model, context, new CancellationToken());
}
