namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExpressionEvaluatorSql(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<ISqlExpressionProvider, SqlExpressionProvider>()
            .AddSingleton<ISqlExpressionProviderHandler, SqlExpressionHandler>()
            .AddSingleton<ISqlExpressionProviderHandler, SqlLikeExpressionHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProvider, EvaluatableSqlExpressionProvider>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, BinaryAndOperatorEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, BinaryOrOperatorEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, ContextEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, DelegateEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, DelegateResultEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, EmptyEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, EqualOperatorEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, GreaterOperatorEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, GreaterOrEqualOperatorEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, LiteralEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, LiteralResultEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, NotEqualOperatorEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, NotNullOperatorEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, NullOperatorEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, PropertyNameEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, SmallerOperatorEvaluatableHandler>()
            .AddSingleton<IEvaluatableSqlExpressionProviderHandler, SmallerOrEqualOperatorEvaluatableHandler>();
}