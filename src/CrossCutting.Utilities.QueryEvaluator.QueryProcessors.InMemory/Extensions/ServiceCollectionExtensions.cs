namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryEvaluatorInMemory(this IServiceCollection serviceCollection)
        => serviceCollection
            .With(x =>
            {
                x.TryAddSingleton<IPaginator, DefaultPaginator>();
                x.TryAddSingleton<IDataFactory, DefaultDataFactory>();
                x.TryAddSingleton<IContextDataFactory, DefaultDataFactory>();
                x.TryAddSingleton<IQueryProcessor, QueryProcessor>();
            });
}
