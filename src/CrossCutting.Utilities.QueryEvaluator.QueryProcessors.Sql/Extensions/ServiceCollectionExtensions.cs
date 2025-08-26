namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryEvaluatorSql(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IQueryProcessor, QueryProcessor>()
            .AddSingleton<IDatabaseCommandProvider<IQuery>, QueryDatabaseCommandProvider>()
            .AddSingleton<IPagedDatabaseCommandProvider<IQuery>, QueryPagedDatabaseCommandProvider>();
}
