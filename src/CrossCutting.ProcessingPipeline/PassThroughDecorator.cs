namespace CrossCutting.ProcessingPipeline;

public class PassThroughDecorator : IPipelineComponentDecorator
{
    public Task<Result> ExecuteAsync<TCommand>(IPipelineComponent<TCommand> component, TCommand command, CancellationToken token)
    {
        component = ArgumentGuard.IsNotNull(component, nameof(component));

        return component.ExecuteAsync(command, token);
    }
}
