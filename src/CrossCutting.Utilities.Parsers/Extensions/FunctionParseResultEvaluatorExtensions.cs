namespace CrossCutting.Utilities.Parsers.Extensions;

public static class FunctionParseResultEvaluatorExtensions
{
    public static Result<object?> Evaluate(this IFunctionParseResultEvaluator instance, FunctionParseResult functionResult)
        => instance.Evaluate(functionResult, null);
}
