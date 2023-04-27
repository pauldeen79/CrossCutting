namespace CrossCutting.Utilities.Parsers.FunctionParserArgumentProcessors;

public class FormattableStringFunctionParserArgumentProcessor : IFunctionParserArgumentProcessor
{
    private readonly IFormattableStringParser _parser;

    public int Order => 10;

    public FormattableStringFunctionParserArgumentProcessor(IFormattableStringParser parser)
    {
        _parser = parser;
    }

    public Result<FunctionParseResultArgument> Process(string stringArgument, IReadOnlyCollection<FunctionParseResult> results, IFormatProvider formatProvider, object? context)
    {
        if (stringArgument.StartsWith("@"))
        {
            var result = _parser.Parse(stringArgument.Substring(1), formatProvider, context);
            return result.IsSuccessful()
                ? Result<FunctionParseResultArgument>.Success(new LiteralArgument(result.Value!))
                : Result<FunctionParseResultArgument>.FromExistingResult(result);
        }

        return Result<FunctionParseResultArgument>.Continue();
    }
}
