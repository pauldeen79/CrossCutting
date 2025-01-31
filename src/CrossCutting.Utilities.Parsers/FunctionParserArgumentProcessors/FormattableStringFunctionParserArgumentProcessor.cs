namespace CrossCutting.Utilities.Parsers.FunctionParserArgumentProcessors;

public class FormattableStringFunctionParserArgumentProcessor : IFunctionParserArgumentProcessor
{
    public Result<IFunctionCallArgument> Process(string argument, IReadOnlyCollection<FunctionCall> functionCalls, FunctionParserSettings settings, object? context)
    {
        settings = settings.IsNotNull(nameof(settings));

        if (argument?.StartsWith("@") == true && settings.FormattableStringParser is not null)
        {
            var result = settings.FormattableStringParser.Parse(argument.Substring(1), new FormattableStringParserSettingsBuilder().WithFormatProvider(settings.FormatProvider).Build(), context);
            return result.IsSuccessful()
                ? Result.Success<IFunctionCallArgument>(new ExpressionArgument(result.Value!.ToString(settings.FormatProvider)))
                : Result.FromExistingResult<IFunctionCallArgument>(result);
        }

        return Result.Continue<IFunctionCallArgument>();
    }
}
