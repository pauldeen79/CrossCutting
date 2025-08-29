namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryEvaluatorSql(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IDatabaseCommandProvider<IQuery>, QueryDatabaseCommandProvider>()
            .AddSingleton<IPagedDatabaseCommandProvider<IQuery>, QueryPagedDatabaseCommandProvider>()
            .AddSingleton<IQueryFieldInfoProvider, QueryFieldInfoProvider>()
            .AddSingleton<IQueryProcessor, QueryProcessor>()
            .AddSingleton<ISqlConditionExpressionProvider, SqlConditionExpressionProvider>()
            .AddSingleton<ISqlExpressionProvider, SqlExpressionProvider>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, BetweenConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, EqualConditionHandler>()
            .AddSingleton<ISqlExpressionProviderHandler, ContextExpressionHandler>()
            .AddSingleton<ISqlExpressionProviderHandler, DelegateExpressionHandler>()
            .AddSingleton<ISqlExpressionProviderHandler, LiteralExpressionHandler>()
            .AddSingleton<ISqlExpressionProviderHandler, PropertyNameExpressionHandler>();
}
