namespace CrossCutting.Utilities.Parsers.FunctionParserArgumentProcessors;

public class FormattableStringFunctionParserArgumentProcessor : IFunctionParserArgumentProcessor
{
    public int Order => 10;

    public Result<FunctionParseResultArgument> Process(string stringArgument, IReadOnlyCollection<FunctionParseResult> results, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser)
    {
        stringArgument = ArgumentGuard.IsNotNull(stringArgument, nameof(stringArgument));

        if (stringArgument.StartsWith("@") && formattableStringParser is not null)
        {
            var result = formattableStringParser.Parse(stringArgument.Substring(1), new FormattableStringParserSettingsBuilder().WithFormatProvider(formatProvider).Build(), context);
            return result.IsSuccessful()
                ? Result.Success<FunctionParseResultArgument>(new LiteralArgument(result.Value!.ToString(formatProvider)))
                : Result.FromExistingResult<FunctionParseResultArgument>(result);
        }

        return Result.Continue<FunctionParseResultArgument>();
    }
}
