namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

public class PipedExpressionProcessor : IExpressionStringParserProcessor
{
    public int Order => 180;

    public Result<object?> Process(ExpressionStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        return BaseProcessor.SplitDelimited(state, '|', split =>
        {
            var resultValue = state.Context;
            foreach (var item in split)
            {
                var result = state.Parser.Parse($"={item}", state.FormatProvider, resultValue, state.FormattableStringParser);
                if (!result.IsSuccessful())
                {
                    return result;
                }
                resultValue = result.Value;
            }

            return Result.Success(resultValue);
        });
    }

    public Result Validate(ExpressionStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        return BaseProcessor.SplitDelimited
        (
            state,
            '|',
            split => Result.Aggregate(split.Select(item => state.Parser.Validate($"={item}", state.FormatProvider, state.Context, state.FormattableStringParser)), Result.Success(), validationResults => Result.Invalid("Validation failed, see inner results for details", validationResults))
        );
    }
}
