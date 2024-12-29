namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

public class ConcatenateExpressionProcessor : IExpressionStringParserProcessor
{
    public int Order => 190;

    public Result<object?> Process(ExpressionStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        return BaseProcessor.SplitDelimited(state, '&', split =>
        {
            var builder = new StringBuilder();
            foreach (var item in split)
            {
                var result = state.Parser.Parse($"={item}", state.FormatProvider, state.Context, state.FormattableStringParser);
                if (!result.IsSuccessful())
                {
                    return result;
                }
                builder.Append(result.Value.ToString(state.FormatProvider));
            }

            return Result.Success<object?>(builder.ToString());
        });
    }

    public Result Validate(ExpressionStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        return BaseProcessor.SplitDelimited(state, '&', split =>
        {
            foreach (var item in split)
            {
                var result = state.Parser.Validate($"={item}", state.FormatProvider, state.Context, state.FormattableStringParser);
                if (!result.IsSuccessful())
                {
                    return result;
                }
            }

            return Result.Success();
        });
    }
}
