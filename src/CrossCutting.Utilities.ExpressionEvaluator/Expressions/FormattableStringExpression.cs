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
            .AddPartResults(results.Value?.Select((x, index) => new ExpressionParsePartResultBuilder().WithErrorMessage(x.ErrorMessage).WithStatus(x.Status).AddValidationErrors(x.ValidationErrors).WithPartName(index.ToString(context.Settings.FormatProvider))) ?? [new ExpressionParsePartResultBuilder().WithPartName("Validation").WithStatus(results.Status).WithErrorMessage(results.ErrorMessage).AddValidationErrors(results.ValidationErrors)])
            .WithStatus(hasFailure ? ResultStatus.Invalid : ResultStatus.Ok)
            .WithErrorMessage(hasFailure ? "Validation failed, see part results for more details" : null);
    }

    private static Result<List<Result<GenericFormattableString>>> ProcessRecursive(string format, ExpressionEvaluatorContext context, bool validateOnly, out string remainder)
    {
        // Handle escaped markers (e.g., {{ -> {)
        var escapedStart = PlaceholderStart + PlaceholderStart;
        var escapedEnd = PlaceholderEnd + PlaceholderEnd;

        remainder = format.Replace(escapedStart, "\uE000")  // Temporarily replace escaped start marker
                          .Replace(escapedEnd, "\uE001");

        var results = new List<Result<GenericFormattableString>>();
        do
        {
            var placeholderSignsResult = GetPlaceholderSignsResult(remainder);
            if (!placeholderSignsResult.IsSuccessful())
            {
                return Result.FromExistingResult<List<Result<GenericFormattableString>>>(placeholderSignsResult);
            }

            if (placeholderSignsResult.Value.openIndex == -1)
            {
                break;
            }

            var placeholder = remainder.Substring(placeholderSignsResult.Value.openIndex + PlaceholderStart.Length, placeholderSignsResult.Value.closeIndex - placeholderSignsResult.Value.openIndex - PlaceholderStart.Length);
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
        var start = PlaceholderStart + PlaceholderStart;
        var end = PlaceholderEnd + PlaceholderEnd;

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
        if (s?.StartsWith(PlaceholderStart) == true && s.EndsWith(PlaceholderEnd))
        {
            placeholderResult = EvaluateRecursive(s, context);
        }

        return placeholderResult;
    }

    private static Result<GenericFormattableString> ProcessPlaceholder(ExpressionEvaluatorContext context, bool validateOnly, string placeholder)
    {
        // Replace placeholder with placeholder expression result
        Result<GenericFormattableString> placeholderResult;
        if (!validateOnly)
        {
            placeholderResult = Result.FromExistingResult(context.Evaluate(placeholder), value => new GenericFormattableString(value));
        }
        else
        {
            var parseResult = context.Parse(placeholder);
            placeholderResult = Result.FromExistingResult<GenericFormattableString>(parseResult.ToResult());
        }

        return placeholderResult;
    }

    private static Result<(int openIndex, int closeIndex)> GetPlaceholderSignsResult(string remainder)
    {
        var closeIndex = remainder.LastIndexOf(PlaceholderEnd);
        if (closeIndex == -1)
        {
            if (remainder.LastIndexOf(PlaceholderStart) > -1)
            {
                return Result.Invalid<(int openIndex, int closeIndex)>($"PlaceholderEnd sign '{PlaceholderEnd}' is missing");
            }

            return Result.Success<(int openIndex, int closeIndex)>((-1, -1));
        }

        var openIndex = remainder.LastIndexOf(PlaceholderStart, closeIndex);
        if (openIndex == -1)
        {
            return Result.Invalid<(int openIndex, int closeIndex)>($"PlaceholderStart sign '{PlaceholderStart}' is missing");
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
