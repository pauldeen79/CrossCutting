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

    public Result<FunctionCall> Parse(string function, FunctionParserSettings settings, object? context)
    {
        ArgumentGuard.IsNotNull(settings, nameof(settings));

        if (string.IsNullOrEmpty(function))
        {
            return Result.NotFound<FunctionCall>("No function found");
        }

        if (function.Contains(TemporaryDelimiter))
        {
            return Result.NotSupported<FunctionCall>($"Input cannot contain {TemporaryDelimiter}, as this is used internally for formatting");
        }

        var results = new List<FunctionCall>();
        var remainder = function;
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

            var stringArguments = remainder.Substring(openIndex.Value + 1, closeIndex.Value - openIndex.Value - 1);
            var stringArgumentsSplit = stringArguments
                .SplitDelimited(',', '\"', trimItems: true, leaveTextQualifier: true)
                .Select(RemoveStringQualifiers);

            var arguments = new List<IFunctionCallArgument>();
            var typeArguments = new List<IFunctionCallTypeArgument>();
            var argumentResults = new ResultDictionaryBuilder()
                .Add("Name", () => FindFunctionName(remainder.Substring(0, openIndex.Value)))
                .Add("Arguments", () => AddArguments(results, stringArgumentsSplit, arguments, settings, context))
                .Add("TypeArguments", results => AddTypeArguments(((Result<FunctionNameAndTypeArguments>)results["Name"]).Value!, typeArguments, settings, context))
                .Build();

            var error = argumentResults.GetError();
            if (error is not null)
            {
                return Result.FromExistingResult<FunctionCall>(error);
            }

            var found = $"{argumentResults.GetValue<FunctionNameAndTypeArguments>("Name").RawResult}({stringArguments})";
            remainder = remainder.Replace(found, FormattableString.Invariant($"{TemporaryDelimiter}{results.Count}{TemporaryDelimiter}"));
            results.Add(new FunctionCall(argumentResults.GetValue<FunctionNameAndTypeArguments>("Name").Name.Trim(), arguments, typeArguments));
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

    private Result<FunctionCall> AddArguments(List<FunctionCall> results, IEnumerable<string> argumentsSplit, List<IFunctionCallArgument> arguments, FunctionParserSettings settings, object? context)
    {
        foreach (var argument in argumentsSplit)
        {
            if (argument.StartsWith(TemporaryDelimiter) && argument.EndsWith(TemporaryDelimiter))
            {
                arguments.Add(new FunctionArgument(results[int.Parse(argument.Substring(TemporaryDelimiter.Length, argument.Length - (TemporaryDelimiter.Length * 2)), CultureInfo.InvariantCulture)]));
                continue;
            }

            var processValueResult = _argumentProcessors
                .Select(x => x.Process(argument, results, settings, context))
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
                arguments.Add(string.IsNullOrEmpty(argument)
                    ? new EmptyArgument()
                    : new ExpressionArgument(argument));
            }
        }

        return Result.Continue<FunctionCall>();
    }

    private Result<FunctionCall> AddTypeArguments(FunctionNameAndTypeArguments functionName, List<IFunctionCallTypeArgument> typeArguments, FunctionParserSettings settings, object? context)
    {
        //TODO: Implement this
        return Result.Continue<FunctionCall>();
    }

    private Result<FunctionNameAndTypeArguments> FindFunctionName(string input)
        => _nameProcessors
            .Select(x => x.Process(input))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.NotFound<FunctionNameAndTypeArguments>("No function name found");
}
