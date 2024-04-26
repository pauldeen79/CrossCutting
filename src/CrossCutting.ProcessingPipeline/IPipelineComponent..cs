namespace CrossCutting.ProcessingPipeline;

public interface IPipelineComponent<TRequest>
{
    Task<Result> Process(PipelineContext<TRequest> context, CancellationToken token);
}

public interface IPipelineComponent<TRequest, TResponse>
{
    Task<Result> Process(PipelineContext<TRequest, TResponse> context, CancellationToken token);
}
