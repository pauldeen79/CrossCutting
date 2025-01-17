namespace CrossCutting.ProcessingPipeline;

public interface IPipelineComponent<TRequest>
{
    Task<Result> ProcessAsync(PipelineContext<TRequest> context, CancellationToken token);
}

public interface IPipelineComponent<TRequest, TResponse>
{
    Task<Result> ProcessAsync(PipelineContext<TRequest, TResponse> context, CancellationToken token);
}
