namespace CrossCutting.Utilities.Parsers;

public class ObjectResolver : IObjectResolver
{
    private readonly IEnumerable<IObjectResolverProcessor> _objectResolverProcssors;

    public ObjectResolver(IEnumerable<IObjectResolverProcessor> objectResolverProcssors)
    {
        ArgumentGuard.IsNotNull(objectResolverProcssors, nameof(objectResolverProcssors));

        _objectResolverProcssors = objectResolverProcssors;
    }

    public Result<T> Resolve<T>(object? sourceObject)
    {
        if (sourceObject is null)
        {
            return Result.NotFound<T>();
        }

        return _objectResolverProcssors
            .Select(x => x.Resolve<T>(sourceObject))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.NotFound<T>($"Could not resolve type {typeof(T).FullName} from {sourceObject.GetType().FullName}");
    }
}
