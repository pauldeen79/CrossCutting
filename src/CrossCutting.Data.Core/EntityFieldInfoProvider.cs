namespace CrossCutting.Data.Core;

public class EntityFieldInfoProvider(IEnumerable<IEntityFieldInfoProviderHandler> handlers) : IEntityFieldInfoProvider
{
    private readonly IEntityFieldInfoProviderHandler[] _handlers = ArgumentGuard.IsNotNull(handlers, nameof(handlers)).ToArray();

    public Result<IEntityFieldInfo> Create(object query)
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        return _handlers
            .Select(x => x.Create(query))
            .WhenNotContinue(() => Result.Invalid<IEntityFieldInfo>($"No query field info provider handler found for type: {query.GetType().FullName}"));
    }
}
