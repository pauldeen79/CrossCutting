namespace CrossCutting.Utilities.Parsers;

public class FunctionParser : IFunctionParser
{
    private const string TemporaryDelimiter = "^^";

    private readonly IEnumerable<IFunctionParserNameProcessor> _nameProcessors;
    private readonly IEnumerable<IFunctionParserArgumentProcessor> _argumentProcessors;

    public FunctionParser(
        IEnumerable<IFunctionParserNameProcessor> nameProcessors,
        IEnumerable<IFunctionParserArgumentProcessor> argumentProcessors)
    {
        _nameProcessors = nameProcessors;
        _argumentProcessors = argumentProcessors;
    }

    public Result<FunctionParseResult> Parse(string input, IFormatProvider formatProvider, object? context)
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
            var addArgumentsResult = AddArguments(results, stringArgumentsSplit, arguments, formatProvider, context);
            if (!addArgumentsResult.IsSuccessful())
            {
                return addArgumentsResult;
            }

            var found = $"{nameResult.Value}({stringArguments})";
            remainder = remainder.Replace(found, FormattableString.Invariant($"{TemporaryDelimiter}{results.Count}{TemporaryDelimiter}"));
            results.Add(new FunctionParseResult(nameResult.Value!, arguments));
        } while (remainder.IndexOf("(") > -1 || remainder.IndexOf(")") > -1);

        return remainder.EndsWith(TemporaryDelimiter)
            ? Result<FunctionParseResult>.Success(results.Last())
            : Result<FunctionParseResult>.NotFound("Input has additional characters after last close bracket");
    }

    private Result<FunctionParseResult> AddArguments(List<FunctionParseResult> results, string[] stringArgumentsSplit, List<FunctionParseResultArgument> arguments, IFormatProvider formatProvider, object? context)
    {
        foreach (var stringArgument in stringArgumentsSplit)
        {
            if (stringArgument.StartsWith(TemporaryDelimiter) && stringArgument.EndsWith(TemporaryDelimiter))
            {
                arguments.Add(new FunctionArgument(results[int.Parse(stringArgument.Substring(TemporaryDelimiter.Length, stringArgument.Length - (TemporaryDelimiter.Length * 2)), CultureInfo.InvariantCulture)]));
                continue;
            }

            var processValueResult = _argumentProcessors
                .OrderBy(x => x.Order)
                .Select(x => x.Process(stringArgument, results, formatProvider, context))
                .FirstOrDefault(x => x.Status != ResultStatus.Continue);
            if (processValueResult != null)
            {
                if (!processValueResult.IsSuccessful())
                {
                    return Result<FunctionParseResult>.FromExistingResult(processValueResult);
                }

                arguments.Add(processValueResult.Value!);
            }
            else
            {
                arguments.Add(new LiteralArgument(stringArgument));
            }
        }

        return Result<FunctionParseResult>.Continue();
    }

    private Result<string> FindFunctionName(string input)
        => _nameProcessors
            .OrderBy(x => x.Order)
            .Select(x => x.Process(input))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result<string>.NotFound("No function name found");
}
