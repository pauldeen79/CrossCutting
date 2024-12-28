namespace CrossCutting.Utilities.Parsers;

public class FunctionEvaluator(IEnumerable<IFunctionResultParser> functionResultParsers) : IFunctionEvaluator
{
    private readonly IEnumerable<IFunctionResultParser> _functionResultParsers = functionResultParsers;

    public Result<object?> Evaluate(FunctionCall functionCall, IExpressionParser parser, object? context)
    {
        functionCall = ArgumentGuard.IsNotNull(functionCall, nameof(functionCall));

        return _functionResultParsers
            .Select(x => x.Parse(functionCall, context, this, parser))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.NotSupported<object?>($"Unknown function found: {functionCall.FunctionName}");
    }
}
