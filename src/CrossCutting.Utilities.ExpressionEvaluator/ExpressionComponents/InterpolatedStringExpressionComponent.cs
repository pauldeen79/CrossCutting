namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class InterpolatedStringExpressionComponent : IExpressionComponent<GenericFormattableString>
{
    private const string TemporaryDelimiter = "\uE002";

    public int Order => 12;

    public async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await EvaluateTypedAsync(context, token).ConfigureAwait(false);

    public async Task<Result<GenericFormattableString>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
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

        return await EvaluateRecursiveAsync(context.Expression.Substring(2, context.Expression.Length - 3), context, token).ConfigureAwait(false);
    }

    public async Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithSourceExpression(context.Expression)
            .WithExpressionComponentType(typeof(InterpolatedStringExpressionComponent))
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

        return await ParseRecursiveAsync(result, context.Expression.Substring(2, context.Expression.Length - 3), context, token).ConfigureAwait(false);
    }

    private static async Task<Result<GenericFormattableString>> EvaluateRecursiveAsync(string format, ExpressionEvaluatorContext context, CancellationToken token)
    {
        var results = (await ProcessRecursiveAsync(format, context, false, token).ConfigureAwait(false)).EnsureNotNull().EnsureValue();

        if (!results.IsSuccessful())
        {
            return Result.FromExistingResult<GenericFormattableString>(results);
        }

        return Result.Success(new GenericFormattableString(results.Value!.Remainder, [.. results.Value.Results.Select(x => x.Value?.ToString(context.Settings.FormatProvider))!]));
    }

    private static async Task<ExpressionParseResult> ParseRecursiveAsync(ExpressionParseResultBuilder result, string format, ExpressionEvaluatorContext context, CancellationToken token)
    {
        var results = (await ProcessRecursiveAsync(format, context, true, token).ConfigureAwait(false)).EnsureNotNull().EnsureValue();
        var hasFailure = !results.IsSuccessful() || results.Value!.Results.Any(x => !x.IsSuccessful());

        return result
            .AddPartResults(results.Value?.Results.Select((x, index) => new ExpressionParsePartResultBuilder().FillFromResult(x).WithPartName(index.ToString(context.Settings.FormatProvider))) ?? [new ExpressionParsePartResultBuilder().WithPartName("Validation").FillFromResult(results)])
            .WithStatus(hasFailure
                ? ResultStatus.Invalid
                : ResultStatus.Ok)
            .WithErrorMessage(hasFailure
                ? "Validation failed, see part results for more details"
                : null);
    }

    private static async Task<Result<ProcessResult>> ProcessRecursiveAsync(string format, ExpressionEvaluatorContext context, bool validateOnly, CancellationToken token)
    {
        // Handle escaped markers (e.g., {{ -> {)
        var escapedStart = context.Settings.PlaceholderStart + context.Settings.PlaceholderStart;
        var escapedEnd = context.Settings.PlaceholderEnd + context.Settings.PlaceholderEnd;

        var remainder = format.Replace(escapedStart, "\uE000")  // Temporarily replace escaped start marker
                              .Replace(escapedEnd, "\uE001");

        var results = new List<Result<GenericFormattableString>>();
        do
        {
            var placeholderSignsResult = GetPlaceholderSignsResult(context, remainder);
            if (!placeholderSignsResult.IsSuccessful())
            {
                return Result.FromExistingResult<ProcessResult>(placeholderSignsResult);
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
                    return Result.Invalid<ProcessResult>("Missing expression");
                }

                results.Add(Result.Invalid<GenericFormattableString>("Missing expression"));
                remainder = remainder.Substring(0, placeholderSignsResult.Value.openIndex)
                    + $"{TemporaryDelimiter}{results.Count}{TemporaryDelimiter}"
                    + remainder.Substring(placeholderSignsResult.Value.closeIndex + context.Settings.PlaceholderEnd.Length);
                continue;
            }

            remainder = remainder.Substring(0, placeholderSignsResult.Value.openIndex)
                + $"{TemporaryDelimiter}{results.Count}{TemporaryDelimiter}"
                + remainder.Substring(placeholderSignsResult.Value.closeIndex + context.Settings.PlaceholderEnd.Length);

            var placeholderResult = await ProcessPlaceholder(context, validateOnly, placeholder, token).ConfigureAwait(false);

            if (!placeholderResult.IsSuccessful())
            {
                if (!validateOnly)
                {
                    return Result.FromExistingResult<ProcessResult>(placeholderResult);
                }

                results.Add(Result.FromExistingResult<GenericFormattableString>(placeholderResult));
                break;
            }

            placeholderResult = await HandleRecursion(context, placeholderResult, token).ConfigureAwait(false);

            results.Add(placeholderResult);
        } while (true);

        remainder = EscapeBraces(context, remainder);

        // Restore escaped markers
        // First, fix invalid format string {bla} to {{bla}} when escaped, else the ToString operation on FormattableString fails
        var start = context.Settings.PlaceholderStart + context.Settings.PlaceholderStart;
        var end = context.Settings.PlaceholderEnd + context.Settings.PlaceholderEnd;

        remainder = remainder.Replace("\uE000", start)
                             .Replace("\uE001", end);

        return Result.Success(new ProcessResult(ReplaceTemporaryDelimiters(remainder, results), results));
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

    private static async Task<Result<GenericFormattableString>> HandleRecursion(ExpressionEvaluatorContext context, Result<GenericFormattableString> placeholderResult, CancellationToken token)
    {
        string? s = placeholderResult.Value!;
        if (s?.Contains(context.Settings.PlaceholderStart) == true && s.Contains(context.Settings.PlaceholderEnd))
        {
            placeholderResult = await EvaluateRecursiveAsync(s, context, token).ConfigureAwait(false);
        }

        return placeholderResult;
    }

    // Replace placeholder with placeholder expression result
    private static async Task<Result<GenericFormattableString>> ProcessPlaceholder(ExpressionEvaluatorContext context, bool validateOnly, string placeholder, CancellationToken token)
        => !validateOnly
            ? Result.FromExistingResult(await context.EvaluateAsync(placeholder, token).ConfigureAwait(false), value => new GenericFormattableString(value))
            : Result.FromExistingResult<GenericFormattableString>(await context.ParseAsync(placeholder, token).ConfigureAwait(false));

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

    private sealed class ProcessResult
    {
        public ProcessResult(string remainder, List<Result<GenericFormattableString>> results)
        {
            Remainder = remainder;
            Results = results;
        }

        public string Remainder { get; }
        public List<Result<GenericFormattableString>> Results { get; }
    }
}
