namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryEvaluatorInMemory(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IPaginator, DefaultPaginator>()
            .AddSingleton<IDataFactory, DefaultDataFactory>()
            .AddSingleton<IContextDataFactory, DefaultDataFactory>()
            .AddSingleton<IQueryProcessor, QueryProcessor>();
}
