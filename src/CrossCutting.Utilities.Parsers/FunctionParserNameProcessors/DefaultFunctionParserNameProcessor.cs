namespace CrossCutting.Utilities.Parsers.FunctionParserNameProcessors;

public class DefaultFunctionParserNameProcessor : IFunctionParserNameProcessor
{
    public Result<string> Process(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return Result.NotFound<string>("No function name found");
        }

        var bracketIndex = input.LastIndexOf("(");
        var commaIndex = input.LastIndexOf(",");
        var greatestIndex = new[] { bracketIndex, commaIndex }.Max();

        if (greatestIndex > -1)
        {
            return Result.Success(input.Substring(greatestIndex + 1));
        }

        return Result.Success(input);
    }
}
