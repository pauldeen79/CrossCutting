namespace CrossCutting.ProcessingPipeline;

public interface IPipeline<TRequest>
{
    Task<Result> ProcessAsync(TRequest request, CancellationToken token);
}

public interface IPipeline<TRequest, TResponse>
{
    Task<Result<TResponse>> ProcessAsync(TRequest request, TResponse seed, CancellationToken token);
}
