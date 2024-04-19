namespace CrossCutting.ProcessingPipeline;

public static class PipelineExtensions
{
    public static Task<Result> Process<TModel>(this IPipeline<TModel> pipeline, TModel model)
        => pipeline.Process(model, CancellationToken.None);

    public static Task<Result> Process<TModel, TContext>(this IPipeline<TModel, TContext> pipeline, TModel model, TContext context)
        => pipeline.Process(model, context, CancellationToken.None);
}
