namespace CrossCutting.Utilities.Parsers;

public class FunctionEvaluator(IEnumerable<IFunctionResultParser> functionResultParsers) : IFunctionEvaluator
{
    private readonly IEnumerable<IFunctionResultParser> _functionResultParsers = functionResultParsers;

    public Result<object?> Evaluate(FunctionCall functionCall, IExpressionParser parser, object? context)
    {
        if (functionCall is null)
        {
            return Result.Invalid<object?>("Function call is required");
        }

        return _functionResultParsers
            .Select(x => x.Parse(functionCall, context, this, parser))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.NotSupported<object?>($"Unknown function call: {functionCall.FunctionName}");
    }

    public Result Validate(FunctionCall functionCall, IExpressionParser parser, object? context)
    {
        if (functionCall is null)
        {
            return Result.Invalid("Function call is required");
        }

        return _functionResultParsers
            .Select(x => x.Validate(functionCall, context, this, parser))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Invalid($"Unknown function call: {functionCall.FunctionName}");
    }
}
