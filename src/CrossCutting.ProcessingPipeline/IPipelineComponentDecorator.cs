namespace CrossCutting.ProcessingPipeline;

public interface IPipelineComponentDecorator
{
    Task<Result> ExecuteAsync<TCommand>(IPipelineComponent<TCommand> component, TCommand command, ICommandService commandService, CancellationToken token);
    Task<Result> ExecuteAsync<TCommand, TResponse>(IPipelineComponent<TCommand, TResponse> component, TCommand command, TResponse response, ICommandService commandService, CancellationToken token);
}
