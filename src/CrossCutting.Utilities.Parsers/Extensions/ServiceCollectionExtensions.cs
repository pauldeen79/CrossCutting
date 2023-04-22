namespace CrossCutting.Utilities.Parsers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddParsers(this IServiceCollection services)
        => services
        .AddSingleton<IExpressionParser, DefaultExpressionParser>()
        .AddSingleton<IExpressionStringParser, ExpressionStringParser>()
        .AddSingleton<IExpressionStringParserProcessor, EmptyExpressionProcessor>()
        .AddSingleton<IExpressionStringParserProcessor, LiteralExpressionProcessor>()
        .AddSingleton<IExpressionStringParserProcessor, OnlyEqualsExpressionProcessor>()
        .AddSingleton<IExpressionStringParserProcessor, FormattableStringExpressionProcessor>()
        .AddSingleton<IExpressionStringParserProcessor, MathematicExpressionProcessor>()
        .AddSingleton<IFormattableStringParser, FormattableStringParser>()
        .AddSingleton<IMathematicExpressionParser, MathematicExpressionParser>();
}
