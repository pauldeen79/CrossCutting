namespace CrossCutting.ProcessingPipeline;

public class PipelineResponseGenerator : IPipelineResponseGenerator
{
    private readonly IEnumerable<IPipelineResponseGeneratorComponent> _responseGeneratorComponents;

    public PipelineResponseGenerator(IEnumerable<IPipelineResponseGeneratorComponent> responseGeneratorComponents)
    {
        ArgumentGuard.IsNotNull(responseGeneratorComponents, nameof(responseGeneratorComponents));

        _responseGeneratorComponents = responseGeneratorComponents.OrderBy(x => (x as IOrderContainer)?.Order).ToArray();
    }

    public Result<T> Generate<T>(object command)
    {
        foreach (var responseGeneratorComponent in _responseGeneratorComponents)
        {
            var result = responseGeneratorComponent.Generate<T>(command);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        var ctor = typeof(T).IsAbstract || typeof(T).IsInterface
            ? null
            : typeof(T).GetConstructors().FirstOrDefault(x => x.GetParameters().Length == 0);

        if (ctor is not null)
        {
            return Result.Success(Activator.CreateInstance<T>());
        }

        return Result.NotSupported<T>($"Response of type {typeof(T)} could not be constructed");
    }
}
