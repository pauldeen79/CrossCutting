namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryEvaluatorSql(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IDatabaseCommandProvider<IQuery>, QueryDatabaseCommandProvider>()
            .AddSingleton<IPagedDatabaseCommandProvider<IQueryContext>, QueryPagedDatabaseCommandProvider>()
            .AddSingleton<IQueryFieldInfoProvider, QueryFieldInfoProvider>()
            .AddSingleton<IQueryProcessor, QueryProcessor>()
            .AddSingleton<ISqlConditionExpressionProvider, SqlConditionExpressionProvider>()
            .AddSingleton<ISqlExpressionProvider, SqlExpressionProvider>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, BetweenConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, EqualConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, GreaterThanConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, GreaterThanOrEqualConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, InConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, NotEqualConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, NotInConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, NotNullConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, NullConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, SmallerThanConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, SmallerThanOrEqualConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, StringContainsConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, StringEndsWithConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, StringEqualsConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, StringNotContainsConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, StringNotEndsWithConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, StringNotEqualsConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, StringNotStartsWithConditionHandler>()
            .AddSingleton<ISqlConditionExpressionProviderHandler, StringStartsWithConditionHandler>()
            .AddSingleton<ISqlExpressionProviderHandler, SqlExpressionHandler>()
            .AddSingleton<ISqlExpressionProviderHandler, SqlLikeExpressionHandler>();
}
