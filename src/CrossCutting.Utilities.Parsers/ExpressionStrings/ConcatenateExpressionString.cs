namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class ConcatenateExpressionString : IExpressionString
{
    public int Order => 190;

    public Result<object?> Evaluate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        return BaseProcessor.SplitDelimited(state, '&', split =>
        {
            var builder = new StringBuilder();
            foreach (var item in split)
            {
                var result = state.Parser.Evaluate($"={item}", state.FormatProvider, state.Context, state.FormattableStringParser);
                if (!result.IsSuccessful())
                {
                    return result;
                }
                builder.Append(result.Value.ToString(state.FormatProvider));
            }

            return Result.Success<object?>(builder.ToString());
        });
    }

    public Result Validate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        return BaseProcessor.SplitDelimited
        (
            state,
            '&',
            split => Result.Aggregate(split.Select(item => state.Parser.Validate($"={item}", state.FormatProvider, state.Context, state.FormattableStringParser)), Result.Success(), validationResults => Result.Invalid("Validation failed, see inner results for details", validationResults))
        );
    }
}
