namespace CrossCutting.Utilities.Parsers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddParsers(this IServiceCollection services)
        => services
        .AddExpressionParser()
        .AddExpressionStringParser()
        .AddFunctionParser()
        .AddFormattableStringParser()
        .AddMathematicExpressionParser()
        .AddObjectResolver()
        .AddVariableProcessor();

    private static IServiceCollection AddExpressionParser(this IServiceCollection services)
        => services
        .AddScoped<IExpressionParser, ExpressionParser>()
        .AddScoped<IExpressionParserProcessor, BooleanExpressionParserProcessor>()
        .AddScoped<IExpressionParserProcessor, ContextExpressionParserProcessor>()
        .AddScoped<IExpressionParserProcessor, NullExpressionParserProcessor>()
        .AddScoped<IExpressionParserProcessor, StringExpressionParserProcessor>()
        .AddScoped<IExpressionParserProcessor, VariableExpressionParserProcessor>()
        .AddScoped<IExpressionParserProcessor, NumericExpressionParserProcessor>()
        .AddScoped<IExpressionParserProcessor, DateTimeExpressionParserProcessor>();

    private static IServiceCollection AddExpressionStringParser(this IServiceCollection services)
        => services
        .AddScoped<IExpressionStringParser, ExpressionStringParser>()
        .AddScoped<IExpressionStringParserProcessor, EmptyExpressionProcessor>()
        .AddScoped<IExpressionStringParserProcessor, PipedExpressionProcessor>()
        .AddScoped<IExpressionStringParserProcessor, ConcatenateExpressionProcessor>()
        .AddScoped<IExpressionStringParserProcessor, LiteralExpressionProcessor>()
        .AddScoped<IExpressionStringParserProcessor, OnlyEqualsExpressionProcessor>()
        .AddScoped<IExpressionStringParserProcessor, FormattableStringExpressionProcessor>()
        .AddScoped<IExpressionStringParserProcessor, MathematicExpressionProcessor>()
        .AddScoped<IExpressionStringParserProcessor, EqualOperator>()
        .AddScoped<IExpressionStringParserProcessor, NotEqualOperator>()
        .AddScoped<IExpressionStringParserProcessor, GreaterThanOperator>()
        .AddScoped<IExpressionStringParserProcessor, GreaterOrEqualThanOperator>()
        .AddScoped<IExpressionStringParserProcessor, SmallerThanOperator>()
        .AddScoped<IExpressionStringParserProcessor, SmallerOrEqualThanOperator>();

    private static IServiceCollection AddFunctionParser(this IServiceCollection services)
        => services
        .AddScoped<IFunctionParser, FunctionParser>()
        .AddScoped<IFunctionParserNameProcessor, DefaultFunctionParserNameProcessor>()
        .AddScoped<IFunctionParserArgumentProcessor, FormattableStringFunctionParserArgumentProcessor>()
        .AddScoped<IFunctionEvaluator, FunctionEvaluator>();

    private static IServiceCollection AddFormattableStringParser(this IServiceCollection services)
        => services
        .AddScoped<IFormattableStringParser, FormattableStringParser>()
        .AddScoped<IPlaceholderProcessor, ExpressionStringPlaceholderProcessor>();

    private static IServiceCollection AddMathematicExpressionParser(this IServiceCollection services)
        => services
        .AddScoped<IMathematicExpressionParser, MathematicExpressionParser>()
        .AddScoped<IMathematicExpressionProcessor, Validate>()
        .AddScoped<IMathematicExpressionProcessor, Recursion>()
        .AddScoped<IMathematicExpressionProcessor, MathematicOperators>()
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
