namespace CrossCutting.Utilities.Parsers.FunctionParserArgumentProcessors;

public class FormattableStringFunctionParserArgumentProcessor : IFunctionParserArgumentProcessor
{
    public int Order => 10;

    public Result<FunctionParseResultArgument> Process(string stringArgument, IReadOnlyCollection<FunctionParseResult> results, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser)
    {
        stringArgument = ArgumentGuard.IsNotNull(stringArgument, nameof(stringArgument));

        if (stringArgument.StartsWith("@") && formattableStringParser is not null)
        {
            var result = formattableStringParser.Parse(stringArgument.Substring(1), formatProvider, context);
            return result.IsSuccessful()
                ? Result<FunctionParseResultArgument>.Success(new LiteralArgument(result.Value!))
                : Result<FunctionParseResultArgument>.FromExistingResult(result);
        }

        return Result<FunctionParseResultArgument>.Continue();
    }
}
