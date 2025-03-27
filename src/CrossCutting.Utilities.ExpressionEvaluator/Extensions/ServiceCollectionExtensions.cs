namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExpressionEvaluator(this IServiceCollection services)
        => services
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddSingleton<IExpressionEvaluator, ExpressionEvaluator>()
            .AddSingleton<IExpression, PrimitiveExpression>()
            .AddSingleton<IExpression, ComparisonOperatorExpression>()
            .AddSingleton<IOperator, EqualOperator>()
            .AddSingleton<IOperator, GreaterOrEqualThanOperator>()
            .AddSingleton<IOperator, GreaterThanOperator>()
            .AddSingleton<IOperator, NotEqualOperator>()
            .AddSingleton<IOperator, SmallerOrEqualThanOperator>()
            .AddSingleton<IOperator, SmallerThanOperator>()
            .AddSingleton<IExpression, BinaryOperatorExpression>()
            .AddSingleton<IExpression, FunctionExpression>();
}
