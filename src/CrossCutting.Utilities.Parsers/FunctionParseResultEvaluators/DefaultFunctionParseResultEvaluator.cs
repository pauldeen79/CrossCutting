namespace CrossCutting.Utilities.Parsers.FunctionParseResultEvaluators;

public class DefaultFunctionParseResultEvaluator : IFunctionParseResultEvaluator
{
    private readonly IEnumerable<IFunctionResultParser> _functionResultParsers;

    public DefaultFunctionParseResultEvaluator(IEnumerable<IFunctionResultParser> functionResultParsers)
    {
        _functionResultParsers = functionResultParsers;
    }

    public Result<object?> Evaluate(FunctionParseResult functionResult, object? context)
        => _functionResultParsers
            .Select(x => x.Parse(functionResult, context, this))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result<object?>.NotSupported($"Unknown function found: {functionResult.FunctionName}");
}
