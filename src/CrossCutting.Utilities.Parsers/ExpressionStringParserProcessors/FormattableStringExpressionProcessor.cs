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
                ? Result.FromExistingResult<FormattableStringParserResult, object?>(state.FormattableStringParser.Parse(state.Input.Substring(3, state.Input.Length - 4), new FormattableStringParserSettingsBuilder().WithFormatProvider(state.FormatProvider).Build(), state.Context), value => value.ToString(state.FormatProvider))
                : Result.Success<object?>(state.Input.Substring(3, state.Input.Length - 4));
        }
        else if (state.Input.StartsWith("=\"") && state.Input.EndsWith("\""))
        {
            // ="string value" -> literal, no functions and no formattable strings possible
            return Result.Success<object?>(state.Input.Substring(2, state.Input.Length - 3));
        }

        return Result.Continue<object?>();
    }
}
