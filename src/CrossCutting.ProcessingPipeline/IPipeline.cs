namespace CrossCutting.ProcessingPipeline;

public interface IPipeline<TRequest>
{
    Task<Result> Process(TRequest request, CancellationToken token);
}

public interface IPipeline<TRequest, TResponse>
{
    Task<Result<TResponse>> Process(TRequest request, TResponse seed, CancellationToken token);
}
