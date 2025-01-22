namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class FormattableStringExpressionString : IExpressionString
{
    public Result<object?> Evaluate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Input.StartsWith("=@\"") && state.Input.EndsWith("\""))
        {
            // =@"string value" -> literal, no functions but formattable strings possible
            return state.FormattableStringParser is not null
                ? Result.FromExistingResult<GenericFormattableString, object?>(state.FormattableStringParser.Parse(state.Input.Substring(3, state.Input.Length - 4), new FormattableStringParserSettingsBuilder().WithFormatProvider(state.FormatProvider).Build(), state.Context), value => value.ToString(state.FormatProvider))
                : Result.Success<object?>(state.Input.Substring(3, state.Input.Length - 4));
        }
        else if (state.Input.StartsWith("=\"") && state.Input.EndsWith("\""))
        {
            // ="string value" -> literal, no functions and no formattable strings possible
            return Result.Success<object?>(state.Input.Substring(2, state.Input.Length - 3));
        }

        return Result.Continue<object?>();
    }

    public Result<Type> Validate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Input.StartsWith("=@\"") && state.Input.EndsWith("\""))
        {
            // =@"string value" -> literal, no functions but formattable strings possible
            return state.FormattableStringParser is not null
                ? Result.FromExistingResult(state.FormattableStringParser.Validate(state.Input.Substring(3, state.Input.Length - 4), new FormattableStringParserSettingsBuilder().WithFormatProvider(state.FormatProvider).Build(), state.Context), typeof(FormattableString))
                : Result.Success(typeof(FormattableString));
        }
        else if (state.Input.StartsWith("=\"") && state.Input.EndsWith("\""))
        {
            // ="string value" -> literal, no functions and no formattable strings possible
            return Result.Success(typeof(FormattableString));
        }

        return Result.Continue<Type>();
    }
}
