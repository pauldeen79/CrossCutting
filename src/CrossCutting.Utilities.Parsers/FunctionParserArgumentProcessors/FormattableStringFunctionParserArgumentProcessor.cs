namespace CrossCutting.Utilities.Parsers.FunctionParserArgumentProcessors;

public class FormattableStringFunctionParserArgumentProcessor : IFunctionParserArgumentProcessor
{
    public int Order => 10;

    public Result<FunctionCallArgument> Process(string argument, IReadOnlyCollection<FunctionCall> functionCalls, IFormatProvider formatProvider, IFormattableStringParser? formattableStringParser, object? context)
    {
        if (argument?.StartsWith("@") == true && formattableStringParser is not null)
        {
            var result = formattableStringParser.Parse(argument.Substring(1), new FormattableStringParserSettingsBuilder().WithFormatProvider(formatProvider).Build(), context);
            return result.IsSuccessful()
                ? Result.Success<FunctionCallArgument>(new LiteralArgument(result.Value!.ToString(formatProvider)))
                : Result.FromExistingResult<FunctionCallArgument>(result);
        }

        return Result.Continue<FunctionCallArgument>();
    }
}
