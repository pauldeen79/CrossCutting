namespace CrossCutting.ProcessingPipeline;

public interface IPipelineComponentDecorator<TRequest>
{
    Task<Result> ProcessAsync(Func<Task<Result>> taskDelegate, TRequest request, CancellationToken token);
}

public interface IPipelineComponentDecorator<TRequest, TResponse>
{
    Task<Result> ProcessAsync(Func<Task<Result>> taskDelegate, TRequest request, TResponse response, CancellationToken token);
}
