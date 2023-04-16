namespace CrossCutting.Utilities.FunctionParser;

public static class FunctionParser
{
    public static Result<FunctionParseResult> Parse(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return Result<FunctionParseResult>.Invalid("Input cannot be null or empty");
        }

        var results = new List<FunctionParseResult>();
        var remainder = input;
        do
        {
            var closeIndex = remainder.IndexOf(")");
            if (closeIndex == -1)
            {
                return Result<FunctionParseResult>.Invalid("Could not find close bracket");
            }

            var openIndex = remainder.LastIndexOf("(", closeIndex);
            if (openIndex == -1)
            {
                return Result<FunctionParseResult>.Invalid("Could not find open bracket");
            }

            var nameResult = FindFunctionName(remainder.Substring(0, openIndex));
            if (!nameResult.IsSuccessful())
            {
                return Result<FunctionParseResult>.FromExistingResult(nameResult);
            }

            var stringArguments = remainder.Substring(openIndex + 1, closeIndex - openIndex - 1);
            var stringArgumentsSplit = stringArguments.SafeSplit(',', '\\', '\\');
            var arguments = new List<FunctionParseResultArgument>();
            AddArguments(results, stringArgumentsSplit, arguments);

            var found = $"{nameResult.Value}({stringArguments})";
            remainder = remainder.Replace(found, FormattableString.Invariant($"##{results.Count}##"));
            results.Add(new FunctionParseResult(nameResult.Value!, arguments));
        } while (remainder.IndexOf("(") > -1 || remainder.IndexOf(")") > -1);

        return Result<FunctionParseResult>.Success(results.Last());
    }

    private static void AddArguments(List<FunctionParseResult> results, string[] stringArgumentsSplit, List<FunctionParseResultArgument> arguments)
    {
        foreach (var stringArgument in stringArgumentsSplit)
        {
            if (stringArgument.StartsWith("##") && stringArgument.EndsWith("##"))
            {
                arguments.Add(new FunctionArgument(results[int.Parse(stringArgument.Substring(2, stringArgument.Length - 4), CultureInfo.InvariantCulture)]));
                continue;
            }
            var parseResult = Parse(stringArgument);
            if (parseResult.IsSuccessful())
            {
                arguments.Add(new FunctionArgument(parseResult.Value!));
            }
            else
            {
                arguments.Add(new LiteralArgument(stringArgument));
            }
        }
    }

    private static Result<string> FindFunctionName(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return Result<string>.Invalid("No function name found");
        }

        var commaIndex = input.LastIndexOf(",");
        if (commaIndex > -1)
        {
            return Result<string>.Success(input.Substring(commaIndex + 1));
        }

        return Result<string>.Success(input);
    }
}
