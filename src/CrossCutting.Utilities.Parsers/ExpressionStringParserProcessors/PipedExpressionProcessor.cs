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

            return Result<object?>.Success(resultValue);
        });
    }
}
