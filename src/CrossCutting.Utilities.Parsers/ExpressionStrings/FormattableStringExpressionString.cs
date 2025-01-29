namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class FormattableStringExpressionString : IExpressionString
{
    public Result<object?> Evaluate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (context.Input.StartsWith("=@\"") && context.Input.EndsWith("\""))
        {
            // =@"string value" -> literal, no functions but formattable strings possible
            return context.FormattableStringParser is not null
                ? Result.FromExistingResult<GenericFormattableString, object?>(context.FormattableStringParser.Parse(context.Input.Substring(3, context.Input.Length - 4), new FormattableStringParserSettingsBuilder().WithFormatProvider(context.Settings.FormatProvider).Build(), context.Context), value => value.ToString(context.Settings.FormatProvider))
                : Result.Success<object?>(context.Input.Substring(3, context.Input.Length - 4));
        }
        else if (context.Input.StartsWith("=\"") && context.Input.EndsWith("\""))
        {
            // ="string value" -> literal, no functions and no formattable strings possible
            return Result.Success<object?>(context.Input.Substring(2, context.Input.Length - 3));
        }

        return Result.Continue<object?>();
    }

    public Result<Type> Validate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (context.Input.StartsWith("=@\"") && context.Input.EndsWith("\""))
        {
            // =@"string value" -> literal, no functions but formattable strings possible
            return context.FormattableStringParser is not null
                ? Result.FromExistingResult(context.FormattableStringParser.Validate(context.Input.Substring(3, context.Input.Length - 4), new FormattableStringParserSettingsBuilder().WithFormatProvider(context.Settings.FormatProvider).Build(), context.Context), typeof(FormattableString))
                : Result.Success(typeof(FormattableString));
        }
        else if (context.Input.StartsWith("=\"") && context.Input.EndsWith("\""))
        {
            // ="string value" -> literal, no functions and no formattable strings possible
            return Result.Success(typeof(FormattableString));
        }

        return Result.Continue<Type>();
    }
}
