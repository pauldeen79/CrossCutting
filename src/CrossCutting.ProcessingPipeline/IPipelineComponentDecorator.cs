namespace CrossCutting.ProcessingPipeline;

public interface IPipelineComponentDecorator
{
    Task<Result> ExecuteAsync<TCommand>(IPipelineComponent<TCommand> component, TCommand command, ICommandService commandService, CancellationToken token);
}
