namespace CrossCutting.Utilities.Parsers;

public class FunctionParseResultEvaluator : IFunctionParseResultEvaluator
{
    private readonly IEnumerable<IFunctionResultParser> _functionResultParsers;

    public FunctionParseResultEvaluator(IEnumerable<IFunctionResultParser> functionResultParsers)
    {
        _functionResultParsers = functionResultParsers;
    }

    public Result<object?> Evaluate(FunctionParseResult functionResult)
        => _functionResultParsers
            .Select(x => x.Parse(functionResult, this))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result<object?>.NotSupported($"Unknown function found: {functionResult.FunctionName}");
}
