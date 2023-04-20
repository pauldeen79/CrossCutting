namespace CrossCutting.Utilities.Parsers;

public static class FunctionParser
{
    private const string TemporaryDelimiter = "^^";

    public static Result<FunctionParseResult> Parse(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return Result<FunctionParseResult>.NotFound("Input cannot be null or empty");
        }

        if (input.Contains(TemporaryDelimiter))
        {
            return Result<FunctionParseResult>.NotSupported($"Input cannot contain {TemporaryDelimiter}, as this is used internally for formatting");
        }

        var results = new List<FunctionParseResult>();
        var remainder = input;
        do
        {
            var closeIndex = remainder.IndexOf(")");
            if (closeIndex == -1)
            {
                return Result<FunctionParseResult>.NotFound("Missing close bracket");
            }

            var openIndex = remainder.LastIndexOf("(", closeIndex);
            if (openIndex == -1)
            {
                return Result<FunctionParseResult>.NotFound("Missing open bracket");
            }

            var nameResult = FindFunctionName(remainder.Substring(0, openIndex));
            if (!nameResult.IsSuccessful())
            {
                return Result<FunctionParseResult>.FromExistingResult(nameResult);
            }

            var stringArguments = remainder.Substring(openIndex + 1, closeIndex - openIndex - 1);
            var stringArgumentsSplit = stringArguments.SplitDelimited(',', '\"');
            var arguments = new List<FunctionParseResultArgument>();
            AddArguments(results, stringArgumentsSplit, arguments);

            var found = $"{nameResult.Value}({stringArguments})";
            remainder = remainder.Replace(found, FormattableString.Invariant($"{TemporaryDelimiter}{results.Count}{TemporaryDelimiter}"));
            results.Add(new FunctionParseResult(nameResult.Value!, arguments));
        } while (remainder.IndexOf("(") > -1 || remainder.IndexOf(")") > -1);

        return Result<FunctionParseResult>.Success(results.Last());
    }

    private static void AddArguments(List<FunctionParseResult> results, string[] stringArgumentsSplit, List<FunctionParseResultArgument> arguments)
    {
        foreach (var stringArgument in stringArgumentsSplit)
        {
            if (stringArgument.StartsWith(TemporaryDelimiter) && stringArgument.EndsWith(TemporaryDelimiter))
            {
                arguments.Add(new FunctionArgument(results[int.Parse(stringArgument.Substring(2, stringArgument.Length - (TemporaryDelimiter.Length * 2)), CultureInfo.InvariantCulture)]));
                continue;
            }

            arguments.Add(new LiteralArgument(stringArgument));
        }
    }

    private static Result<string> FindFunctionName(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return Result<string>.NotFound("No function name found");
        }

        var bracketIndex = input.LastIndexOf("(");
        var commaIndex = input.LastIndexOf(",");
        var greatestIndex = new[] { bracketIndex, commaIndex }.Max();

        if (greatestIndex > -1)
        {
            return Result<string>.Success(input.Substring(greatestIndex + 1));
        }

        return Result<string>.Success(input);
    }
}
