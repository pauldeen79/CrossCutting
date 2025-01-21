namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

internal static class BaseProcessor
{
    internal static Result<T> SplitDelimited<T>(ExpressionStringEvaluatorState state, char splitDelimiter, Func<string[], Result<T>> validDelegate)
    {
        if (state.Input.IndexOf(splitDelimiter) == -1)
        {
            return Result.Continue<T>();
        }

        var split = state.Input.Substring(1).SplitDelimited(splitDelimiter, '\"', true, true);
        if (split.Length == 1)
        {
            return Result.Continue<T>();
        }

        return validDelegate(split);
    }

    internal static Result SplitDelimited(ExpressionStringEvaluatorState state, char splitDelimiter, Func<string[], Result> validDelegate)
    {
        if (state.Input.IndexOf(splitDelimiter) == -1)
        {
            return Result.Continue();
        }

        var split = state.Input.Substring(1).SplitDelimited(splitDelimiter, '\"', true, true);
        if (split.Length == 1)
        {
            return Result.Continue();
        }

        return validDelegate(split);
    }

    internal static Func<string[], Result<Type>> Parse(ExpressionStringEvaluatorState state)
        => split => Result.Aggregate(split.Select(item => state.Parser.Validate($"={item}", state.FormatProvider, state.Context, state.FormattableStringParser)), Result.NoContent<Type>(), validationResults => Result.Invalid<Type>("Validation failed, see inner results for details", validationResults));
}
