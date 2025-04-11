namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class FormattableStringExpression : IExpression<GenericFormattableString>
{
    private const string TemporaryDelimiter = "\uE002";

    public int Order => 12;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

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

        return EvaluateRecursive(context.Expression.Substring(2, context.Expression.Length - 3), context);
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

        return ParseRecursive(result, context.Expression.Substring(2, context.Expression.Length - 3), context);
    }

    private static Result<GenericFormattableString> EvaluateRecursive(string format, ExpressionEvaluatorContext context)
    {
        var results = ProcessRecursive(format, context, false, out var remainder);

        if (!results.IsSuccessful())
        {
            return Result.FromExistingResult<GenericFormattableString>(results);
        }
        
        return Result.Success(new GenericFormattableString(remainder, [.. results.Value.Select(x => x.Value?.ToString(context.Settings.FormatProvider))!]));
    }

    private static ExpressionParseResult ParseRecursive(ExpressionParseResultBuilder result, string format, ExpressionEvaluatorContext context)
    {
        var results = ProcessRecursive(format, context, true, out _);
        var hasFailure = !results.IsSuccessful() || results.Value?.Any(x => !x.Status.IsSuccessful()) == true;

        return result
            .AddPartResults(results.Value?.Select((x, index) => new ExpressionParsePartResultBuilder().FillFromResult(x).WithPartName(index.ToString(context.Settings.FormatProvider))) ?? [new ExpressionParsePartResultBuilder().WithPartName("Validation").FillFromResult(results)])
            .WithStatus(hasFailure
                ? ResultStatus.Invalid
                : ResultStatus.Ok)
            .WithErrorMessage(hasFailure
                ? "Validation failed, see part results for more details"
                : null);
    }

    private static Result<List<Result<GenericFormattableString>>> ProcessRecursive(string format, ExpressionEvaluatorContext context, bool validateOnly, out string remainder)
    {
        // Handle escaped markers (e.g., {{ -> {)
        var escapedStart = context.Settings.PlaceholderStart + context.Settings.PlaceholderStart;
        var escapedEnd = context.Settings.PlaceholderEnd + context.Settings.PlaceholderEnd;

        remainder = format.Replace(escapedStart, "\uE000")  // Temporarily replace escaped start marker
                          .Replace(escapedEnd, "\uE001");

        var results = new List<Result<GenericFormattableString>>();
        do
        {
            var placeholderSignsResult = GetPlaceholderSignsResult(context, remainder);
            if (!placeholderSignsResult.IsSuccessful())
            {
                return Result.FromExistingResult<List<Result<GenericFormattableString>>>(placeholderSignsResult);
            }

            if (placeholderSignsResult.Value.openIndex == -1)
            {
                break;
            }

            var placeholder = remainder.Substring(placeholderSignsResult.Value.openIndex + context.Settings.PlaceholderStart.Length, placeholderSignsResult.Value.closeIndex - placeholderSignsResult.Value.openIndex - context.Settings.PlaceholderStart.Length);
            if (string.IsNullOrWhiteSpace(placeholder))
            {
                if (!validateOnly)
                {
                    return Result.Invalid<List<Result<GenericFormattableString>>>("Missing expression");
                }

                results.Add(Result.Invalid<GenericFormattableString>("Missing expression"));
                remainder = remainder.Substring(0, placeholderSignsResult.Value.openIndex)
                    + $"{TemporaryDelimiter}{results.Count}{TemporaryDelimiter}"
                    + remainder.Substring(placeholderSignsResult.Value.closeIndex + 1);
                continue;
            }

            remainder = remainder.Substring(0, placeholderSignsResult.Value.openIndex)
                + $"{TemporaryDelimiter}{results.Count}{TemporaryDelimiter}"
                + remainder.Substring(placeholderSignsResult.Value.closeIndex + 1);

            var placeholderResult = ProcessPlaceholder(context, validateOnly, placeholder);

            if (!placeholderResult.IsSuccessful())
            {
                if (!validateOnly)
                {
                    return Result.FromExistingResult<List<Result<GenericFormattableString>>>(placeholderResult);
                }

                results.Add(Result.FromExistingResult<GenericFormattableString>(placeholderResult));
                remainder = remainder.Substring(0, placeholderSignsResult.Value.openIndex)
                    + $"{TemporaryDelimiter}{results.Count}{TemporaryDelimiter}"
                    + remainder.Substring(placeholderSignsResult.Value.closeIndex + 1);
                continue;
            }

            placeholderResult = HandleRecursion(context, placeholderResult);

            results.Add(placeholderResult);
        } while (true);

        remainder = EscapeBraces(context, remainder);

        // Restore escaped markers
        // First, fix invalid format string {bla} to {{bla}} when escaped, else the ToString operation on FormattableString fails
        var start = context.Settings.PlaceholderStart + context.Settings.PlaceholderStart;
        var end = context.Settings.PlaceholderEnd + context.Settings.PlaceholderEnd;

        remainder = remainder.Replace("\uE000", start)
                             .Replace("\uE001", end);

        remainder = ReplaceTemporaryDelimiters(remainder, results);

        return Result.Success(results);
    }

    private static string EscapeBraces(ExpressionEvaluatorContext context, string remainder)
    {
        if (context.Settings.EscapeBraces)
        {
            // Fix FormatException when using ToString on FormattableString
            remainder = remainder.Replace("{", "{{")
                                 .Replace("}", "}}");
        }

        return remainder;
    }

    private static Result<GenericFormattableString> HandleRecursion(ExpressionEvaluatorContext context, Result<GenericFormattableString> placeholderResult)
    {
        string? s = placeholderResult.Value!;
        if (s?.StartsWith(context.Settings.PlaceholderStart) == true && s.EndsWith(context.Settings.PlaceholderEnd))
        {
            placeholderResult = EvaluateRecursive(s, context);
        }

        return placeholderResult;
    }

    // Replace placeholder with placeholder expression result
    private static Result<GenericFormattableString> ProcessPlaceholder(ExpressionEvaluatorContext context, bool validateOnly, string placeholder)
        => !validateOnly
            ? Result.FromExistingResult(context.Evaluate(placeholder), value => new GenericFormattableString(value))
            : Result.FromExistingResult<GenericFormattableString>(context.Parse(placeholder).ToResult());

    private static Result<(int openIndex, int closeIndex)> GetPlaceholderSignsResult(ExpressionEvaluatorContext context, string remainder)
    {
        var closeIndex = remainder.LastIndexOf(context.Settings.PlaceholderEnd);
        if (closeIndex == -1)
        {
            if (remainder.LastIndexOf(context.Settings.PlaceholderStart) > -1)
            {
                return Result.Invalid<(int openIndex, int closeIndex)>($"PlaceholderEnd sign '{context.Settings.PlaceholderEnd}' is missing");
            }

            return Result.Success<(int openIndex, int closeIndex)>((-1, -1));
        }

        var openIndex = remainder.LastIndexOf(context.Settings.PlaceholderStart, closeIndex);
        if (openIndex == -1)
        {
            return Result.Invalid<(int openIndex, int closeIndex)>($"PlaceholderStart sign '{context.Settings.PlaceholderStart}' is missing");
        }

        return Result.Success((openIndex, closeIndex));
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
