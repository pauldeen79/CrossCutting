namespace CrossCutting.ProcessingPipeline;

public static class PipelineComponentExtensions
{
    public static Task<Result> ProcessAsync<TRequest>(this IPipelineComponent<TRequest> instance, PipelineContext<TRequest> context)
        => instance.ProcessAsync(context, CancellationToken.None);

    public static Task<Result> ProcessAsync<TRequest, TResponse>(this IPipelineComponent<TRequest, TResponse> instance, PipelineContext<TRequest, TResponse> context)
        => instance.ProcessAsync(context, CancellationToken.None);
}
