namespace CrossCutting.ProcessingPipeline;

public interface IPipelineComponentDecorator<TCommand>
{
    Task<Result> ExecuteAsync(IPipelineComponent<TCommand> component, TCommand command, CancellationToken token);
}
