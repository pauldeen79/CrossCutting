namespace CrossCutting.ProcessingPipeline;

public class PassThroughDecorator<TCommand> : IPipelineComponentDecorator<TCommand>
{
    public Task<Result> ExecuteAsync(IPipelineComponent<TCommand> component, TCommand command, CancellationToken token)
    {
        component = ArgumentGuard.IsNotNull(component, nameof(component));

        return component.ExecuteAsync(command, token);
    }
}
