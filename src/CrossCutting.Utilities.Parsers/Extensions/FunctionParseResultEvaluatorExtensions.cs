namespace CrossCutting.Utilities.Parsers.Extensions;

public static class FunctionParseResultEvaluatorExtensions
{
    public static Result<object?> Evaluate(this IFunctionParseResultEvaluator instance, FunctionCall functionResult, IExpressionParser parser)
        => instance.Evaluate(functionResult, parser, null);
}
