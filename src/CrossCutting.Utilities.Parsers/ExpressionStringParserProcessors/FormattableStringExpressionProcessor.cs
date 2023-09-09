namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

public class FormattableStringExpressionProcessor : IExpressionStringParserProcessor
{
    public int Order => 400;

    public Result<object?> Process(ExpressionStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Input.StartsWith("=@\"") && state.Input.EndsWith("\""))
        {
            // =@"string value" -> literal, no functions but formattable strings possible
            return state.FormattableStringParser is not null
                ? Result<object?>.FromExistingResult(state.FormattableStringParser.Parse(state.Input.Substring(3, state.Input.Length - 4), state.FormatProvider, state.Context))
                : Result<object?>.Success(state.Input.Substring(3, state.Input.Length - 4));
        }
        else if (state.Input.StartsWith("=\"") && state.Input.EndsWith("\""))
        {
            // ="string value" -> literal, no functions and no formattable strings possible
            return Result<object?>.Success(state.Input.Substring(2, state.Input.Length - 3));
        }

        return Result<object?>.Continue();
    }
}
