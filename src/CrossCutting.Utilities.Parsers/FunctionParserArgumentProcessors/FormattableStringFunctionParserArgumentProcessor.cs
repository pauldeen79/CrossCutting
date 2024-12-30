namespace CrossCutting.Utilities.Parsers.FunctionParserArgumentProcessors;

public class FormattableStringFunctionParserArgumentProcessor : IFunctionParserArgumentProcessor
{
    public int Order => 10;

    public Result<FunctionCallArgument> Process(string stringArgument, IReadOnlyCollection<FunctionCall> results, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser)
    {
        //TODO: Review this path
        if (stringArgument is null)
        {
            return Result.Invalid<FunctionCallArgument>("Argument is empty");
        }

        if (stringArgument.StartsWith("@") && formattableStringParser is not null)
        {
            var result = formattableStringParser.Parse(stringArgument.Substring(1), new FormattableStringParserSettingsBuilder().WithFormatProvider(formatProvider).Build(), context);
            return result.IsSuccessful()
                ? Result.Success<FunctionCallArgument>(new LiteralArgument(result.Value!.ToString(formatProvider)))
                : Result.FromExistingResult<FunctionCallArgument>(result);
        }

        return Result.Continue<FunctionCallArgument>();
    }
}
