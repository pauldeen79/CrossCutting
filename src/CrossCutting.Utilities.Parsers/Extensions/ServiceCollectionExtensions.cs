namespace CrossCutting.Utilities.Parsers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddParsers(this IServiceCollection services)
        => services
        .AddExpressionParser()
        .AddExpressionStringParser()
        .AddFunctionParser()
        .AddFunctionDescriptorProvider()
        .AddFormattableStringParser()
        .AddMathematicExpressionParser()
        .AddObjectResolver()
        .AddVariableProcessor();

    private static IServiceCollection AddExpressionParser(this IServiceCollection services)
        => services
        .AddScoped<IExpressionEvaluator, ExpressionEvaluator>()
        .AddScoped<IExpression, BooleanExpressionParserProcessor>()
        .AddScoped<IExpression, ContextExpressionParserProcessor>()
        .AddScoped<IExpression, NullExpressionParserProcessor>()
        .AddScoped<IExpression, StringExpressionParserProcessor>()
        .AddScoped<IExpression, VariableExpressionParserProcessor>()
        .AddScoped<IExpression, NumericExpressionParserProcessor>()
        .AddScoped<IExpression, DateTimeExpressionParserProcessor>();

    private static IServiceCollection AddExpressionStringParser(this IServiceCollection services)
        => services
        .AddScoped<IExpressionStringEvaluator, ExpressionStringEvaluator>()
        .AddScoped<IExpressionString, EmptyExpressionProcessor>()
        .AddScoped<IExpressionString, PipedExpressionProcessor>()
        .AddScoped<IExpressionString, ConcatenateExpressionProcessor>()
        .AddScoped<IExpressionString, LiteralExpressionProcessor>()
        .AddScoped<IExpressionString, OnlyEqualsExpressionProcessor>()
        .AddScoped<IExpressionString, FormattableStringExpressionProcessor>()
        .AddScoped<IExpressionString, MathematicExpressionProcessor>()
        .AddScoped<IExpressionString, EqualOperator>()
        .AddScoped<IExpressionString, NotEqualOperator>()
        .AddScoped<IExpressionString, GreaterThanOperator>()
        .AddScoped<IExpressionString, GreaterOrEqualThanOperator>()
        .AddScoped<IExpressionString, SmallerThanOperator>()
        .AddScoped<IExpressionString, SmallerOrEqualThanOperator>();

    private static IServiceCollection AddFunctionParser(this IServiceCollection services)
        => services
        .AddScoped<IFunctionParser, FunctionParser>()
        .AddScoped<IFunctionParserNameProcessor, DefaultFunctionParserNameProcessor>()
        .AddScoped<IFunctionParserArgumentProcessor, FormattableStringFunctionParserArgumentProcessor>()
        .AddScoped<IFunctionEvaluator, FunctionEvaluator>();

    private static IServiceCollection AddFunctionDescriptorProvider(this IServiceCollection services)
        => services
        .AddScoped<IFunctionDescriptorProvider, FunctionDescriptorProvider>();

    private static IServiceCollection AddFormattableStringParser(this IServiceCollection services)
        => services
        .AddScoped<IFormattableStringParser, FormattableStringParser>()
        .AddScoped<IPlaceholder, ExpressionStringPlaceholder>();

    private static IServiceCollection AddMathematicExpressionParser(this IServiceCollection services)
        => services
        .AddScoped<IMathematicExpressionEvaluator, MathematicExpressionEvaluator>()
        .AddScoped<IMathematicExpression, Validate>()
        .AddScoped<IMathematicExpression, Recursion>()
        .AddScoped<IMathematicExpression, MathematicOperators>()
        .AddScoped<IMathematicExpressionValidator, NullOrEmptyValidator>()
        .AddScoped<IMathematicExpressionValidator, TemporaryDelimiterValidator>()
        .AddScoped<IMathematicExpressionValidator, StartWithOperatorValidator>()
        .AddScoped<IMathematicExpressionValidator, EndWithOperatorValidator>()
        .AddScoped<IMathematicExpressionValidator, EmptyValuePartValidator>()
        .AddScoped<IMathematicExpressionValidator, BraceValidator>();

    private static IServiceCollection AddObjectResolver(this IServiceCollection services)
        => services
        .AddScoped<IObjectResolver, ObjectResolver>();

    private static IServiceCollection AddVariableProcessor(this IServiceCollection services)
        => services
        .AddScoped<IVariableProcessor, VariableProcessor>();
}
