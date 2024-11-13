namespace CrossCutting.Utilities.Parsers.FunctionParseResultEvaluators;

public class DefaultFunctionParseResultEvaluator(IEnumerable<IFunctionResultParser> functionResultParsers) : IFunctionParseResultEvaluator
{
    private readonly IEnumerable<IFunctionResultParser> _functionResultParsers = functionResultParsers;

    public Result<object?> Evaluate(FunctionParseResult functionResult, IExpressionParser parser, object? context)
    {
        functionResult = ArgumentGuard.IsNotNull(functionResult, nameof(functionResult));

        return _functionResultParsers
            .Select(x => x.Parse(functionResult, context, this, parser))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.NotSupported<object?>($"Unknown function found: {functionResult.FunctionName}");
    }
}
