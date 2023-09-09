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

            return Result<object?>.Success(builder.ToString());
        });
    }
}
