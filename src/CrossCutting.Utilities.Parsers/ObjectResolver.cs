namespace CrossCutting.Utilities.Parsers;

public class ObjectResolver : IObjectResolver
{
    private readonly IEnumerable<IObjectResolverProcessor> _objectResolverProcessors;

    public ObjectResolver(IEnumerable<IObjectResolverProcessor> objectResolverProcessors)
    {
        ArgumentGuard.IsNotNull(objectResolverProcessors, nameof(objectResolverProcessors));

        _objectResolverProcessors = objectResolverProcessors;
    }

    public Result<T> Resolve<T>(object? sourceObject)
    {
        if (sourceObject is null)
        {
            return Result.NotFound<T>();
        }

        return _objectResolverProcessors
            .Select(x => x.Resolve<T>(sourceObject))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                .WhenNull(ResultStatus.NotFound, $"Could not resolve type {typeof(T).FullName} from {sourceObject.GetType().FullName}");
    }
}
