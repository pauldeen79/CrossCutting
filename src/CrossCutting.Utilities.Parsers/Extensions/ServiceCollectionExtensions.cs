namespace CrossCutting.Utilities.Parsers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddParsers(this IServiceCollection services)
        => services
            .AddExpressionEvaluator()
            .AddExpressionStringEvaluator()
            .AddFunctionParser()
            .AddFormattableStringParser()
            .AddMathematicExpressionParser()
            .AddObjectResolver()
            .AddVariableProcessor();

    private static IServiceCollection AddExpressionEvaluator(this IServiceCollection services)
        => services
            .AddScoped<IExpressionEvaluator, ExpressionEvaluator>()
            .AddScoped<IExpression, OperatorExpression>()
            .AddScoped<IExpression, BooleanExpression>()
            .AddScoped<IExpression, ContextExpression>()
            .AddScoped<IExpression, NullExpression>()
            .AddScoped<IExpression, StringExpression>()
            .AddScoped<IExpression, VariableExpression>()
            .AddScoped<IExpression, NumericExpression>()
            .AddScoped<IExpression, DateTimeExpression>()
            .AddScoped<IExpression, TypeOfExpression>()
            .AddScoped<IExpression, MathematicExpression>();

    private static IServiceCollection AddExpressionStringEvaluator(this IServiceCollection services)
        => services
            .AddScoped<IExpressionStringEvaluator, ExpressionStringEvaluator>()
            .AddScoped<IExpressionString, EmptyExpressionString>()
            .AddScoped<IExpressionString, EqualOperator>()
            .AddScoped<IExpressionString, NotEqualOperator>()
            .AddScoped<IExpressionString, GreaterOrEqualThanOperator>()
            .AddScoped<IExpressionString, GreaterThanOperator>()
            .AddScoped<IExpressionString, SmallerOrEqualThanOperator>()
            .AddScoped<IExpressionString, SmallerThanOperator>()
            .AddScoped<IExpressionString, PipedExpressionString>()
            .AddScoped<IExpressionString, ConcatenateExpressionString>()
            .AddScoped<IExpressionString, LiteralExpressionString>()
            .AddScoped<IExpressionString, OnlyEqualsExpressionString>()
            .AddScoped<IExpressionString, FormattableStringExpressionString>()
            .AddScoped<IExpressionString, MathematicExpressionString>()
            .AddScoped<IExpressionString, CastExpressionString>();

    private static IServiceCollection AddFunctionParser(this IServiceCollection services)
        => services
            .AddScoped<IFunctionParser, FunctionParser>()
            .AddScoped<IFunctionParserNameProcessor, DefaultFunctionParserNameProcessor>()
            .AddScoped<IFunctionParserArgumentProcessor, FormattableStringFunctionParserArgumentProcessor>()
            .AddScoped<IFunctionEvaluator, FunctionEvaluator>()
            .AddScoped<IFunctionDescriptorProvider, FunctionDescriptorProvider>()
            .AddScoped<IFunctionDescriptorMapper, FunctionDescriptorMapper>()
            .AddScoped<IFunctionCallArgumentValidator, FunctionCallArgumentValidator>();

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
            .AddScoped<IVariableEvaluator, VariableEvaluator>();
}
