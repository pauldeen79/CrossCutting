namespace CrossCutting.ProcessingPipeline;

public interface IPipeline<TRequest>
{
    Task<Result> Process(TRequest request, CancellationToken token);
}

public interface IPipeline<TRequest, TResponse>
    where TResponse : new()
{
    Task<Result<TResponse>> Process(TRequest request, CancellationToken token);
}
