namespace CrossCutting.Utilities.Parsers.FunctionParserNameProcessors;

public class DefaultFunctionParserNameProcessor : IFunctionParserNameProcessor
{
    public Result<FunctionNameAndTypeArguments> Process(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return Result.NotFound<FunctionNameAndTypeArguments>("No function name found");
        }

        var bracketIndex = input.LastIndexOf("(");
        var commaIndex = input.LastIndexOf(",");
        var greatestIndex = new[] { bracketIndex, commaIndex }.Max();

        if (greatestIndex > -1)
        {
            return Result.Success(ParseName(input.Substring(greatestIndex + 1)));
        }

        return Result.Success(ParseName(input));
    }

    private static FunctionNameAndTypeArguments ParseName(string input)
        => new FunctionNameAndTypeArguments(input, input.Trim().RemoveGenerics(), input.Trim().GetGenericArguments().Split([','], StringSplitOptions.RemoveEmptyEntries));
}
