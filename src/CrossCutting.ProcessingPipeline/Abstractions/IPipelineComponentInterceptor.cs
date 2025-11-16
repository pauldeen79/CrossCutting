namespace CrossCutting.ProcessingPipeline.Abstractions;

public interface IPipelineComponentInterceptor
{
    Task<Result> ExecuteAsync<TCommand>(TCommand command, ICommandService commandService, Func<Task<Result>> next, CancellationToken token);
    Task<Result> ExecuteAsync<TCommand, TResponse>(TCommand command, TResponse response, ICommandService commandService, Func<Task<Result>> next, CancellationToken token);
}
