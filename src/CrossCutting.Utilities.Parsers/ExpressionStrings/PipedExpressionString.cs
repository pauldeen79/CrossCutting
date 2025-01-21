namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class PipedExpressionString : IExpressionString
{
    public Result<object?> Evaluate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        return BaseProcessor.SplitDelimited(state, '|', split =>
        {
            var resultValue = state.Context;
            foreach (var item in split)
            {
                var result = state.Parser.Evaluate($"={item}", state.FormatProvider, resultValue, state.FormattableStringParser);
                if (!result.IsSuccessful())
                {
                    return result;
                }
                resultValue = result.Value;
            }

            return Result.Success(resultValue);
        });
    }

    public Result<Type> Validate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        return BaseProcessor.SplitDelimited
        (
            state,
            '|',
            split => Result.Aggregate(split.Select(item => state.Parser.Validate($"={item}", state.FormatProvider, state.Context, state.FormattableStringParser)), Result.NoContent<Type>(), validationResults => Result.Invalid<Type>("Validation failed, see inner results for details", validationResults))
        );
    }
}
