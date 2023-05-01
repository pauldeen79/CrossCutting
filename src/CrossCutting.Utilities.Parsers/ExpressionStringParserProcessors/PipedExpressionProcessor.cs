namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

public class PipedExpressionProcessor : IExpressionStringParserProcessor
{
    public int Order => 180;

    public Result<object?> Process(ExpressionStringParserState state)
    {
        if (state.Input.IndexOf('|') == -1)
        {
            return Result<object?>.Continue();
        }

        var split = state.Input.Substring(1).SplitDelimited('|', '\"', true, true);
        if (split.Length == 1)
        {
            // The pipe sign is within a string, so we need to continue
            return Result<object?>.Continue();
        }
        
        var resultValue = state.Context;
        foreach (var item in split)
        {
            var result = state.Parser.Parse($"={item}", state.FormatProvider, resultValue);
            if (!result.IsSuccessful())
            {
                return result;
            }
            resultValue = result.Value;
        }

        return Result<object?>.Success(resultValue);
    }
}
