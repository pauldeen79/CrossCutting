namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExpressionEvaluator(this IServiceCollection services)
        => services
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddSingleton<IExpressionEvaluator, ExpressionEvaluator>()
            .AddSingleton<IExpression, PrimitiveExpression>()
            .AddSingleton<IExpression, StringExpression>()
            .AddSingleton<IExpression, FormattableStringExpression>()
            .AddSingleton<IExpression, TypeOfExpression>()
            .AddSingleton<IExpression, FunctionExpression>()
            .AddSingleton<IFunctionCallArgumentValidator, FunctionCallArgumentValidator>()
            .AddSingleton<IFunctionDescriptorMapper, FunctionDescriptorMapper>()
            .AddSingleton<IFunctionDescriptorProvider, FunctionDescriptorProvider>()
            .AddSingleton<IFunctionParser, FunctionParser>()
            .AddSingleton<IFunctionResolver, FunctionResolver>()
            .AddSingleton<IExpression, NumericExpression>()
            .AddSingleton<IExpression, OperatorExpression>()
            .AddSingleton<IOperatorExpressionTokenizer, OperatorExpressionTokenizer>()
            .AddSingleton<IOperatorExpressionParser, OperatorExpressionParser>();
}
