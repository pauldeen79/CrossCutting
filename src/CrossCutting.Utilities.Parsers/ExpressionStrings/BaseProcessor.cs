namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

internal static class BaseProcessor
{
    internal static Result<T> SplitDelimited<T>(ExpressionStringEvaluatorContext context, char splitDelimiter, Func<string[], Result<T>> validDelegate)
    {
        if (context.Input.IndexOf(splitDelimiter) == -1)
        {
            return Result.Continue<T>();
        }

        var split = context.Input.Substring(1).SplitDelimited(splitDelimiter, '\"', true, true);
        if (split.Length == 1)
        {
            return Result.Continue<T>();
        }

        return validDelegate(split);
    }

    internal static Result SplitDelimited(ExpressionStringEvaluatorContext context, char splitDelimiter, Func<string[], Result> validDelegate)
    {
        if (context.Input.IndexOf(splitDelimiter) == -1)
        {
            return Result.Continue();
        }

        var split = context.Input.Substring(1).SplitDelimited(splitDelimiter, '\"', true, true);
        if (split.Length == 1)
        {
            return Result.Continue();
        }

        return validDelegate(split);
    }

    internal static Func<string[], Result<Type>> Parse(ExpressionStringEvaluatorContext context)
        => split => Result.Aggregate(split.Select(item => context.Parser.Validate($"={item}", context.FormatProvider, context.Context, context.FormattableStringParser)), Result.NoContent<Type>(), validationResults => Result.Invalid<Type>("Validation failed, see inner results for details", validationResults));
}
