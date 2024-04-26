namespace CrossCutting.ProcessingPipeline;

public static class PipelineExtensions
{
    public static Task<Result> Process<TRequest>(this IPipeline<TRequest> pipeline, TRequest request)
        => pipeline.Process(request, CancellationToken.None);

    public static Task<Result<TResponse>> Process<TRequest, TResponse>(this IPipeline<TRequest, TResponse> pipeline, TRequest request)
        where TResponse : new()
        => pipeline.Process(request, CancellationToken.None);
}
