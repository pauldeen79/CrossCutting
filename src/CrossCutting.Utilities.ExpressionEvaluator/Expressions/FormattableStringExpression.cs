namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class FormattableStringExpression : IExpression<GenericFormattableString>
{
    private const string TemporaryDelimiter = "\uE002";
    private const string PlaceholderStart = "{";
    private const string PlaceholderEnd = "}";

    public int Order => 12;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
        => EvaluateTyped(context).Transform<object?>(x => x);

    public Result<GenericFormattableString> EvaluateTyped(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));


        if (context.Expression.Length < 3 || !context.Expression.StartsWith("@\"") || !context.Expression.EndsWith("\""))
        {
            return Result.Continue<GenericFormattableString>();
        }

        return ProcessRecursive(context.Expression.Substring(2, context.Expression.Length - 3), context, false);
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithSourceExpression(context.Expression)
            .WithExpressionType(typeof(FormattableStringExpression))
            .WithResultType(typeof(IFormattable));

        if (context.Expression.Length < 3 || !context.Expression.StartsWith("@\"") || !context.Expression.EndsWith("\""))
        {
            return result.WithStatus(ResultStatus.Continue);
        }

        var processResult = ProcessRecursive(context.Expression.Substring(2, context.Expression.Length - 3), context, true);

        return result
            .WithStatus(processResult.Status)
            .WithErrorMessage(processResult.ErrorMessage)
            .AddValidationErrors(processResult.ValidationErrors);
    }

    private Result<GenericFormattableString> ProcessRecursive(string format, ExpressionEvaluatorContext context, bool validateOnly)
    {
        if (string.IsNullOrEmpty(format))
        {
            return Result.Success(new GenericFormattableString(string.Empty, []));
        }

        // Handle escaped markers (e.g., {{ -> {)
        var escapedStart = PlaceholderStart + PlaceholderStart;
        var escapedEnd = PlaceholderEnd + PlaceholderEnd;

        var remainder = format.Replace(escapedStart, "\uE000") // Temporarily replace escaped start marker
                              .Replace(escapedEnd, "\uE001");  // Temporarily replace escaped end marker

        var results = new List<Result<GenericFormattableString>>();
        do
        {
            var closeIndex = remainder.LastIndexOf(PlaceholderEnd);
            if (closeIndex == -1)
            {
                break;
            }

            var openIndex = remainder.LastIndexOf(PlaceholderStart);
            if (openIndex == -1)
            {
                return Result.Invalid<GenericFormattableString>($"PlaceholderStart sign '{PlaceholderStart}' is missing");
            }

            var placeholder = remainder.Substring(openIndex + PlaceholderStart.Length, closeIndex - openIndex - PlaceholderStart.Length);
            var found = $"{PlaceholderStart}{placeholder}{PlaceholderEnd}";
            remainder = remainder.Replace(found, $"{TemporaryDelimiter}{results.Count}{TemporaryDelimiter}");
            var placeholderResult = ProcessOnPlaceholders(context, validateOnly, placeholder);

            placeholderResult = CombineResults(placeholderResult, validateOnly, () => ProcessRecursive(format, context, placeholderResult, validateOnly));
            if (!placeholderResult.IsSuccessful() && !validateOnly)
            {
                return placeholderResult;
            }

            results.Add(placeholderResult);
        } while (NeedToRepeat(remainder));

        if (context.Settings.EscapeBraces)
        {
            // Fix FormatException when using ToString on FormattableString
            remainder = remainder.Replace("{", "{{")
                                 .Replace("}", "}}");
        }

        // Restore escaped markers
        // First, fix invalid format string {bla} to {{bla}} when escaped, else the ToString operation on FormattableString fails
        var start = PlaceholderStart.StartsWith("{")
            ? PlaceholderStart + PlaceholderStart
            : PlaceholderStart;
        var end = PlaceholderEnd.StartsWith("}")
            ? PlaceholderEnd + PlaceholderEnd
            : PlaceholderEnd;

        remainder = remainder.Replace("\uE000", start)
                             .Replace("\uE001", end);

        remainder = ReplaceTemporaryDelimiters(remainder, results);

        return validateOnly
            ? Result.Aggregate(results, Result.Success(new GenericFormattableString(string.Empty, [])), validationResults => Result.Invalid<GenericFormattableString>("Validation failed, see inner results for details", validationResults))
            : Result.Success(new GenericFormattableString(remainder, [.. results.Where(x => x.Value is not null).Select(x => x.Value!.ToString(context.Settings.FormatProvider))]));
    }

    private Result<GenericFormattableString> ProcessRecursive(string format, ExpressionEvaluatorContext context, Result<GenericFormattableString> placeholderResult, bool validateOnly)
    {
        if (placeholderResult.Value?.Format == "{0}"
            && placeholderResult.Value.ArgumentCount == 1
            && placeholderResult.Value.GetArgument(0) is string placeholderResultValue
            && NeedRecurse(format, placeholderResultValue)) //compare with input to prevent infinitive loop
        {

            placeholderResult = ProcessRecursive(placeholderResultValue, context, validateOnly);
        }

        return placeholderResult;
    }

    private static Result<GenericFormattableString> ProcessOnPlaceholders(ExpressionEvaluatorContext context, bool validateOnly, string value)
        //TODO: Add something like 'validate only'? a.k.a. context.Validate/Parse
        => Result.FromExistingResult(context.Evaluate(value), value => new GenericFormattableString(value));

    private static string ReplaceTemporaryDelimiters(string remainder, List<Result<GenericFormattableString>> results)
    {
        for (var i = 0; i < results.Count; i++)
        {
            remainder = remainder.Replace($"{TemporaryDelimiter}{i}{TemporaryDelimiter}", $"{{{i}}}");
        }

        return remainder;
    }

    private static Result<GenericFormattableString> CombineResults(Result<GenericFormattableString> placeholderResult, bool validateOnly, Func<Result<GenericFormattableString>> dlg)
    {
        if (!placeholderResult.IsSuccessful() && !validateOnly)
        {
            return placeholderResult;
        }

        return dlg();
    }

    private static bool NeedToRepeat(string remainder)
        => remainder.IndexOf(PlaceholderStart) > -1
        || remainder.IndexOf(PlaceholderEnd) > -1;

    private static bool NeedRecurse(string format, string placeholderResultValue)
        => placeholderResultValue.Contains(PlaceholderStart)
        && placeholderResultValue.Contains(PlaceholderEnd)
        && placeholderResultValue != format;
}
