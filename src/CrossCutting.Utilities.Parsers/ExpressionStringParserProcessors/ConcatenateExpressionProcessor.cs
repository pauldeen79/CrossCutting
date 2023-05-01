namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

public class ConcatenateExpressionProcessor : IExpressionStringParserProcessor
{
    public int Order => 190;

    public Result<object?> Process(ExpressionStringParserState state)
    {
        if (state.Input.IndexOf('&') == -1)
        {
            return Result<object?>.Continue();
        }

        var split = state.Input.Substring(1).SplitDelimited('&', '\"', true, true);
        if (split.Length == 1)
        {
            return Result<object?>.Continue();
        }

        var builder = new StringBuilder();
        foreach (var item in split)
        {
            var result = state.Parser.Parse($"={item}", state.FormatProvider, state.Context);
            if (!result.IsSuccessful())
            {
                return result;
            }
            builder.Append(result.Value.ToString(state.FormatProvider));
        }

        return Result<object?>.Success(builder.ToString());
    }
}
