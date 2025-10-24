namespace CrossCutting.ProcessingPipeline;

public class PassThroughDecorator<TRequest> : IPipelineComponentDecorator<TRequest>
{
    public Task<Result> ProcessAsync(Func<Task<Result>> taskDelegate, TRequest request, CancellationToken token)
    {
        taskDelegate = ArgumentGuard.IsNotNull(taskDelegate, nameof(taskDelegate));

        return taskDelegate();
    }
}
public class PassThroughDecorator<TRequest, TResponse> : IPipelineComponentDecorator<TRequest, TResponse>
{
    public Task<Result> ProcessAsync(Func<Task<Result>> taskDelegate, TRequest request, TResponse response, CancellationToken token)
    {
        taskDelegate = ArgumentGuard.IsNotNull(taskDelegate, nameof(taskDelegate));

        return taskDelegate();
    }
}
