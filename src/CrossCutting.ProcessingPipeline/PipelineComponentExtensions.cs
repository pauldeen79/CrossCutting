namespace CrossCutting.ProcessingPipeline;

public static class PipelineComponentExtensions
{
    public static Task<Result> Process<TRequest>(this IPipelineComponent<TRequest> instance, PipelineContext<TRequest> context)
        => instance.Process(context, CancellationToken.None);

    public static Task<Result> Process<TRequest, TResponse>(this IPipelineComponent<TRequest, TResponse> instance, PipelineContext<TRequest, TResponse> context)
        => instance.Process(context, CancellationToken.None);
}
