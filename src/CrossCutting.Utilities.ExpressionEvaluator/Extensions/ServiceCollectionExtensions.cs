namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExpressionEvaluator(this IServiceCollection services)
        => services
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddSingleton<IExpressionEvaluator, ExpressionEvaluator>()
            .AddSingleton<IExpressionComponent, PrimitiveExpressionComponent>()
            .AddSingleton<IExpressionComponent, StringExpressionComponent>()
            .AddSingleton<IExpressionComponent, FormattableStringExpressionComponent>()
            .AddSingleton<IExpressionComponent, TypeOfExpressionComponent>()
            .AddSingleton<IExpressionComponent, FunctionExpressionComponent>()
            .AddSingleton<IFunctionCallArgumentValidator, FunctionCallArgumentValidator>()
            .AddSingleton<IFunctionDescriptorMapper, FunctionDescriptorMapper>()
            .AddSingleton<IFunctionDescriptorProvider, FunctionDescriptorProvider>()
            .AddSingleton<IFunctionParser, FunctionParser>()
            .AddSingleton<IFunctionResolver, FunctionResolver>()
            .AddSingleton<IExpressionComponent, NumericExpressionComponent>()
            .AddSingleton<IExpressionTokenizer, ExpressionTokenizer>()
            .AddSingleton<IExpressionParser, ExpressionParser>();
}
