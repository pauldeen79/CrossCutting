namespace CrossCutting.ProcessingPipeline;

public class Pipeline<TCommand> : IPipeline<TCommand>
{
    private readonly IPipelineComponentDecorator<TCommand> _decorator;
    private readonly IEnumerable<IPipelineComponent<TCommand>> _components;

    public Pipeline(IPipelineComponentDecorator<TCommand> decorator, IEnumerable<IPipelineComponent<TCommand>> components)
    {
        ArgumentGuard.IsNotNull(decorator, nameof(decorator));
        ArgumentGuard.IsNotNull(components, nameof(components));

        _decorator = decorator;
        _components = components.OrderBy(x => (x as IOrderContainer)?.Order).ToList();
    }

    public async Task<Result> ExecuteAsync(TCommand command, CancellationToken token)
    {
        ArgumentGuard.IsNotNull(command, nameof(command));

        var results = new List<Result>();
        foreach (var component in _components)
        {
            var result = await _decorator.ExecuteAsync(component, command, token)
                .ConfigureAwait(false);

            results.Add(result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return Result.Aggregate
        (
            results,
            Result.Success(),
            errors => Result.Error(errors, "An error occured while processing the pipeline. See the inner results for more details.")
        );
    }
}
