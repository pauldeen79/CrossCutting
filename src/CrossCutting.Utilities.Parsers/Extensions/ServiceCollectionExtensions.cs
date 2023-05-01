namespace CrossCutting.Utilities.Parsers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddParsers(this IServiceCollection services)
        => services
        .AddExpressionParser()
        .AddExpressionStringParser()
        .AddFunctionParser()
        .AddFormattableStringParser()
        .AddMathematicExpressionParser();

    private static IServiceCollection AddExpressionParser(this IServiceCollection services)
        => services
        .AddSingleton<IExpressionParser, ExpressionParser>()
        .AddSingleton<IExpressionParserProcessor, BooleanExpressionParserProcessor>()
        .AddSingleton<IExpressionParserProcessor, ContextExpressionParserProcessor>()
        .AddSingleton<IExpressionParserProcessor, NullExpressionParserProcessor>()
        .AddSingleton<IExpressionParserProcessor, StringExpressionParserProcessor>()
        .AddSingleton<IExpressionParserProcessor, NumericExpressionParserProcessor>()
        .AddSingleton<IExpressionParserProcessor, DateTimeExpressionParserProcessor>();

    private static IServiceCollection AddExpressionStringParser(this IServiceCollection services)
        => services
        .AddSingleton<IExpressionStringParser, ExpressionStringParser>()
        .AddSingleton<IExpressionStringParserProcessor, EmptyExpressionProcessor>()
        .AddSingleton<IExpressionStringParserProcessor, PipedExpressionProcessor>()
        .AddSingleton<IExpressionStringParserProcessor, ConcatenateExpressionProcessor>()
        .AddSingleton<IExpressionStringParserProcessor, LiteralExpressionProcessor>()
        .AddSingleton<IExpressionStringParserProcessor, OnlyEqualsExpressionProcessor>()
        .AddSingleton<IExpressionStringParserProcessor, FormattableStringExpressionProcessor>()
        .AddSingleton<IExpressionStringParserProcessor, MathematicExpressionProcessor>()
        .AddSingleton<IExpressionStringParserProcessor, EqualOperator>()
        .AddSingleton<IExpressionStringParserProcessor, NotEqualOperator>()
        .AddSingleton<IExpressionStringParserProcessor, GreaterThanOperator>()
        .AddSingleton<IExpressionStringParserProcessor, GreaterOrEqualThanOperator>()
        .AddSingleton<IExpressionStringParserProcessor, SmallerThanOperator>()
        .AddSingleton<IExpressionStringParserProcessor, SmallerOrEqualThanOperator>();

    private static IServiceCollection AddFunctionParser(this IServiceCollection services)
        => services
        .AddSingleton<IFunctionParser, FunctionParser>()
        .AddSingleton<IFunctionParserNameProcessor, DefaultFunctionParserNameProcessor>()
        .AddSingleton<IFunctionParserArgumentProcessor, FormattableStringFunctionParserArgumentProcessor>()
        .AddSingleton<IFunctionParseResultEvaluator, FunctionParseResultEvaluator>();

    private static IServiceCollection AddFormattableStringParser(this IServiceCollection services)
        => services
        .AddSingleton<IFormattableStringParser, FormattableStringParser>()
        .AddSingleton<IFormattableStringStateProcessor, OpenSignProcessor>()
        .AddSingleton<IFormattableStringStateProcessor, CloseSignProcessor>()
        .AddSingleton<IFormattableStringStateProcessor, PlaceholderProcessor>()
        .AddSingleton<IFormattableStringStateProcessor, ResultProcessor>()
        .AddSingleton<IPlaceholderProcessor, UnknownPlaceholderProcessor>(); // used by CloseSignProcessor

    private static IServiceCollection AddMathematicExpressionParser(this IServiceCollection services)
        => services
        .AddSingleton<IMathematicExpressionParser, MathematicExpressionParser>()
        .AddSingleton<IMathematicExpressionProcessor, Validate>()
        .AddSingleton<IMathematicExpressionProcessor, Recursion>()
        .AddSingleton<IMathematicExpressionProcessor, MathematicOperators>()
        .AddSingleton<IMathematicExpressionValidator, NullOrEmptyValidator>()
        .AddSingleton<IMathematicExpressionValidator, TemporaryDelimiterValidator>()
        .AddSingleton<IMathematicExpressionValidator, StartWithOperatorValidator>()
        .AddSingleton<IMathematicExpressionValidator, EndWithOperatorValidator>()
        .AddSingleton<IMathematicExpressionValidator, EmptyValuePartValidator>()
        .AddSingleton<IMathematicExpressionValidator, BraceValidator>();
}
