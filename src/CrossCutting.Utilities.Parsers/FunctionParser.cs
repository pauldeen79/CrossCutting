namespace CrossCutting.Utilities.Parsers;

public class FunctionParser : IFunctionParser
{
    private const string TemporaryDelimiter = "\uE002";

    private readonly IEnumerable<IFunctionParserNameProcessor> _nameProcessors;
    private readonly IEnumerable<IFunctionParserArgumentProcessor> _argumentProcessors;

    public FunctionParser(
        IEnumerable<IFunctionParserNameProcessor> nameProcessors,
        IEnumerable<IFunctionParserArgumentProcessor> argumentProcessors)
    {
        ArgumentGuard.IsNotNull(nameProcessors, nameof(nameProcessors));
        ArgumentGuard.IsNotNull(argumentProcessors, nameof(argumentProcessors));

        _nameProcessors = nameProcessors;
        _argumentProcessors = argumentProcessors;
    }

    public Result<FunctionCall> Parse(string input, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser)
    {
        ArgumentGuard.IsNotNull(formatProvider, nameof(formatProvider));

        if (string.IsNullOrEmpty(input))
        {
            return Result.NotFound<FunctionCall>("No function found");
        }

        if (input.Contains(TemporaryDelimiter))
        {
            return Result.NotSupported<FunctionCall>($"Input cannot contain {TemporaryDelimiter}, as this is used internally for formatting");
        }

        var results = new List<FunctionCall>();
        var remainder = input;
        do
        {
            var quoteMap = BuildQuoteMap(remainder);
            var closeIndex = remainder.Select((character, index) => new { character, index }).FirstOrDefault(x => x.character == ')' && !IsInQuoteMap(x.index, quoteMap))?.index;
            if (closeIndex is null)
            {
                return Result.NotFound<FunctionCall>("Missing close bracket");
            }

            var openIndex = remainder.Select((character, index) => new { character, index }).LastOrDefault(x => x.index < closeIndex.Value && x.character == '(' && !IsInQuoteMap(x.index, quoteMap))?.index;
            if (openIndex is null)
            {
                return Result.NotFound<FunctionCall>("Missing open bracket");
            }

            var nameResult = FindFunctionName(remainder.Substring(0, openIndex.Value));
            if (!nameResult.IsSuccessful())
            {
                return Result.FromExistingResult<FunctionCall>(nameResult);
            }

            var stringArguments = remainder.Substring(openIndex.Value + 1, closeIndex.Value - openIndex.Value - 1);
            var stringArgumentsSplit = stringArguments
                .SplitDelimited(',', '\"', trimItems: true, leaveTextQualifier: true)
                .Select(RemoveStringQualifiers);

            var arguments = new List<FunctionCallArgument>();
            var addArgumentsResult = AddArguments(results, stringArgumentsSplit, arguments, formatProvider, context, formattableStringParser);
            if (!addArgumentsResult.IsSuccessful())
            {
                return addArgumentsResult;
            }

            var found = $"{nameResult.Value}({stringArguments})";
            remainder = remainder.Replace(found, FormattableString.Invariant($"{TemporaryDelimiter}{results.Count}{TemporaryDelimiter}"));
            results.Add(new FunctionCall(nameResult.Value!.Trim(), arguments, formatProvider, context));
        } while (remainder.IndexOf("(") > -1 || remainder.IndexOf(")") > -1);

        return remainder.EndsWith(TemporaryDelimiter)
            ? Result.Success(results[results.Count - 1])
            : Result.NotFound<FunctionCall>("Input has additional characters after last close bracket");
    }

    private static string RemoveStringQualifiers(string value)
    {
        if (value.StartsWith("\"") && value.EndsWith("\""))
        {
            return value.Substring(1, value.Length - 2);
        }

        if (value.StartsWith("@\"") && value.EndsWith("\""))
        {
            return "@" + value.Substring(2, value.Length - 3);
        }

        return value;
    }

    private static bool IsInQuoteMap(int index, IEnumerable<(int StartIndex, int EndIndex)> quoteMap)
        => quoteMap.Any(x => x.StartIndex < index && x.EndIndex > index);

    private static IEnumerable<(int StartIndex, int EndIndex)> BuildQuoteMap(string value)
    {
        var inText = false;
        var index = -1;
        var lastQuote = -1;

        foreach (var character in value)
        {
            index++;
            if (character == '\"')
            {
                inText = !inText;
                if (inText)
                {
                    lastQuote = index;
                }
                else
                {
                    yield return new(lastQuote, index);
                }
            }
        }
    }

    private Result<FunctionCall> AddArguments(List<FunctionCall> results, IEnumerable<string> stringArgumentsSplit, List<FunctionCallArgument> arguments, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser)
    {
        foreach (var stringArgument in stringArgumentsSplit)
        {
            if (stringArgument.StartsWith(TemporaryDelimiter) && stringArgument.EndsWith(TemporaryDelimiter))
            {
                arguments.Add(new RecursiveArgument(results[int.Parse(stringArgument.Substring(TemporaryDelimiter.Length, stringArgument.Length - (TemporaryDelimiter.Length * 2)), CultureInfo.InvariantCulture)]));
                continue;
            }

            var processValueResult = _argumentProcessors
                .OrderBy(x => x.Order)
                .Select(x => x.Process(stringArgument, results, formatProvider, context, formattableStringParser))
                .FirstOrDefault(x => x.Status != ResultStatus.Continue);
            if (processValueResult is not null)
            {
                if (!processValueResult.IsSuccessful())
                {
                    return Result.FromExistingResult<FunctionCall>(processValueResult);
                }

                arguments.Add(processValueResult.Value!);
            }
            else
            {
                arguments.Add(new LiteralArgument(stringArgument));
            }
        }

        return Result.Continue<FunctionCall>();
    }

    private Result<string> FindFunctionName(string input)
        => _nameProcessors
            .OrderBy(x => x.Order)
            .Select(x => x.Process(input))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.NotFound<string>("No function name found");
}
