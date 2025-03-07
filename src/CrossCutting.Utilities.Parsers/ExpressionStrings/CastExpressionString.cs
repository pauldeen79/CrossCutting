namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class CastExpressionString : IExpressionString
{
    private static readonly Regex _castRegEx = new(@"^=cast\s+(?<expression>\S+)\s+as\s+(?<type>\S+)$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));

    public Result<object?> Evaluate(ExpressionStringEvaluatorContext context)
    {
        context = context.IsNotNull(nameof(context));

        var match = _castRegEx.Match(context.Input);
        if (match.Success)
        {
            var expressionInput = match.Groups["expression"].Value;
            var typeInput = match.Groups["type"].Value;

            var type = Type.GetType(typeInput, false);
            if (type is null)
            {
                return Result.Invalid<object?>($"Unknown type: {typeInput}");
            }

            var expressionResult = context.Evaluator.Evaluate($"={expressionInput}", context.Settings, context.Context, context.FormattableStringParser);
            if (!expressionResult.IsSuccessful())
            {
                return expressionResult;
            }

            return Result.Success<object?>(Convert.ChangeType(expressionResult.Value, type!, context.Settings.FormatProvider));
        }
        else
        {
            return Result.Continue<object?>();
        }
    }

    public Result<Type> Validate(ExpressionStringEvaluatorContext context)
    {
        context = context.IsNotNull(nameof(context));

        var match = _castRegEx.Match(context.Input);
        if (match.Success)
        {
            var typeInput = match.Groups["type"].Value;

            var type = Type.GetType(typeInput, false);
            if (type is null)
            {
                return Result.Invalid<Type>($"Unknown type: {typeInput}");
            }

            return Result.NoContent<Type>();
        }
        else
        {
            return Result.Continue<Type>();
        }
    }
}
