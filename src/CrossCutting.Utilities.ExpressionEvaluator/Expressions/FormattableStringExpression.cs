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

        if (context.Expression.Length < 3 || !context.Expression.StartsWith("$\""))
        {
            return Result.Continue<GenericFormattableString>();
        }

        if (!context.Expression.EndsWith("\""))
        {
            return Result.Invalid<GenericFormattableString>("FormattableString is not closed correctly");
        }

        return ProcessRecursive(context.Expression.Substring(2, context.Expression.Length - 3), context);
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithSourceExpression(context.Expression)
            .WithExpressionType(typeof(FormattableStringExpression))
            .WithResultType(typeof(IFormattable));

        if (context.Expression.Length < 3 || !context.Expression.StartsWith("$\""))
        {
            return result.WithStatus(ResultStatus.Continue);
        }

        if (!context.Expression.EndsWith("\""))
        {
            return result
                .WithStatus(ResultStatus.Invalid)
                .WithErrorMessage("FormattableString is not closed correctly");
        }

        var processResult = ProcessRecursive(context.Expression.Substring(2, context.Expression.Length - 3), context);

        return result
            .WithStatus(processResult.Status)
            .WithErrorMessage(processResult.ErrorMessage)
            .AddValidationErrors(processResult.ValidationErrors);
    }

    private static Result<GenericFormattableString> ProcessRecursive(string format, ExpressionEvaluatorContext context)
    {
        // Handle escaped markers (e.g., {{ -> {)
        var escapedStart = PlaceholderStart + PlaceholderStart;
        var escapedEnd = PlaceholderEnd + PlaceholderEnd;

        var remainder = format.Replace(escapedStart, "\uE000")  // Temporarily replace escaped start marker
                              .Replace(escapedEnd,   "\uE001"); // Temporarily replace escaped end marker

        var results = new List<Result<GenericFormattableString>>();
        do
        {
            var closeIndex = remainder.LastIndexOf(PlaceholderEnd);
            if (closeIndex == -1)
            {
                if (remainder.LastIndexOf(PlaceholderStart) > -1)
                {
                    return Result.Invalid<GenericFormattableString>($"PlaceholderEnd sign '{PlaceholderEnd}' is missing");
                }
                break;
            }

            var openIndex = remainder.LastIndexOf(PlaceholderStart, closeIndex);
            if (openIndex == -1)
            {
                return Result.Invalid<GenericFormattableString>($"PlaceholderStart sign '{PlaceholderStart}' is missing");
            }

            var placeholder = remainder.Substring(openIndex + PlaceholderStart.Length, closeIndex - openIndex - PlaceholderStart.Length);
            if (string.IsNullOrWhiteSpace(placeholder))
            {
                return Result.Invalid<GenericFormattableString>("Missing expression");
            }

            var found = $"{PlaceholderStart}{placeholder}{PlaceholderEnd}";
            remainder = remainder.Replace(found, $"{TemporaryDelimiter}{results.Count}{TemporaryDelimiter}");

            var placeholderResult = Result.FromExistingResult(context.Evaluate(placeholder), value => new GenericFormattableString(value));
            if (!placeholderResult.IsSuccessful())
            {
                return placeholderResult;
            }

            placeholderResult = HandleRecursion(context, placeholderResult);

            results.Add(placeholderResult);
        } while (true);

        if (context.Settings.EscapeBraces)
        {
            // Fix FormatException when using ToString on FormattableString
            remainder = remainder.Replace("{", "{{")
                                 .Replace("}", "}}");
        }

        // Restore escaped markers
        // First, fix invalid format string {bla} to {{bla}} when escaped, else the ToString operation on FormattableString fails
        var start = PlaceholderStart + PlaceholderStart;
        var end = PlaceholderEnd + PlaceholderEnd;

        remainder = remainder.Replace("\uE000", start)
                             .Replace("\uE001", end);

        remainder = ReplaceTemporaryDelimiters(remainder, results);

        return Result.Success(new GenericFormattableString(remainder, [.. results.Select(x => x.Value?.ToString(context.Settings.FormatProvider))!]));
    }

    private static Result<GenericFormattableString> HandleRecursion(ExpressionEvaluatorContext context, Result<GenericFormattableString> placeholderResult)
    {
        string? s = placeholderResult.Value!;
        if (s?.StartsWith(PlaceholderStart) == true && s.EndsWith(PlaceholderEnd))
        {
            placeholderResult = ProcessRecursive(s, context);
        }

        return placeholderResult;
    }

    private static string ReplaceTemporaryDelimiters(string remainder, List<Result<GenericFormattableString>> results)
    {
        for (var i = 0; i < results.Count; i++)
        {
            remainder = remainder.Replace($"{TemporaryDelimiter}{i}{TemporaryDelimiter}", $"{{{i}}}");
        }

        return remainder;
    }
}
