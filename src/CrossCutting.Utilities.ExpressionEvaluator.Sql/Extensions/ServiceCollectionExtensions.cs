namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExpressionEvaluatorSql(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<ISqlExpressionProvider, SqlExpressionProvider>()
            .AddSingleton<ISqlExpressionProviderHandler, SqlExpressionHandler>()
            .AddSingleton<ISqlExpressionProviderHandler, SqlLikeExpressionHandler>();
}