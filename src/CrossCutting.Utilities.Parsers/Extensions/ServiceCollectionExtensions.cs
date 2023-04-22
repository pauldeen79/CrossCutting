namespace CrossCutting.Utilities.Parsers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddParsers(this IServiceCollection services)
        => services
        .AddSingleton<IExpressionParser, ExpressionParser>()
        .AddSingleton<IExpressionStringParser, ExpressionStringParser>()
        .AddSingleton<IExpressionStringParserProcessor, EmptyExpressionProcessor>()
        .AddSingleton<IExpressionStringParserProcessor, LiteralExpressionProcessor>()
        .AddSingleton<IExpressionStringParserProcessor, OnlyEqualsExpressionProcessor>()
        .AddSingleton<IExpressionStringParserProcessor, FormattableStringExpressionProcessor>()
        .AddSingleton<IFormattableStringStateProcessor, OpenSignProcessor>()
        .AddSingleton<IFormattableStringStateProcessor, CloseSignProcessor>()
        .AddSingleton<IFormattableStringStateProcessor, PlaceholderProcessor>()
        .AddSingleton<IFormattableStringStateProcessor, ResultProcessor>()
        .AddSingleton<IExpressionStringParserProcessor, MathematicExpressionProcessor>()
        .AddSingleton<IFormattableStringParser, FormattableStringParser>()
        .AddSingleton<IMathematicExpressionParser, MathematicExpressionParser>()
        .AddSingleton<IMathematicExpressionProcessor, Validate>()
        .AddSingleton<IMathematicExpressionProcessor, Recursion>()
        .AddSingleton<IMathematicExpressionProcessor, Operators>()
        .AddSingleton<IMathematicExpressionValidator, NullOrEmptyValidator>()
        .AddSingleton<IMathematicExpressionValidator, TemporaryDelimiterValidator>()
        .AddSingleton<IMathematicExpressionValidator, StartWithOperatorValidator>()
        .AddSingleton<IMathematicExpressionValidator, EndWithOperatorValidator>()
        .AddSingleton<IMathematicExpressionValidator, EmptyValuePartValidator>()
        .AddSingleton<IMathematicExpressionValidator, BracketValidator>();
}
