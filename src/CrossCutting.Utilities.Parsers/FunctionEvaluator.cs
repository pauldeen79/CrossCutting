namespace CrossCutting.Utilities.Parsers;

public class FunctionEvaluator(IEnumerable<IFunctionResultParser> functionResultParsers) : IFunctionEvaluator
{
    private readonly IEnumerable<IFunctionResultParser> _functionResultParsers = functionResultParsers;

    public Result<object?> Evaluate(FunctionCall functionResult, IExpressionParser parser, object? context)
    {
        functionResult = ArgumentGuard.IsNotNull(functionResult, nameof(functionResult));

        return _functionResultParsers
            .Select(x => x.Parse(functionResult, context, this, parser))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.NotSupported<object?>($"Unknown function found: {functionResult.FunctionName}");
    }
}
