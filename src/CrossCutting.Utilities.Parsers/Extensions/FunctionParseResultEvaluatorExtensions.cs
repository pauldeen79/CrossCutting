namespace CrossCutting.Utilities.Parsers.Extensions;

public static class FunctionParseResultEvaluatorExtensions
{
    public static Result<object?> Evaluate(this IFunctionEvaluator instance, FunctionCall functionResult, IExpressionParser parser)
        => instance.Evaluate(functionResult, parser, null);

    public static Result Validate(this IFunctionEvaluator instance, FunctionCall functionResult, IExpressionParser parser)
        => instance.Validate(functionResult, parser, null);
}
