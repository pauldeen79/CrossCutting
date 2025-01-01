namespace CrossCutting.Utilities.Parsers;

public class FunctionEvaluator(IEnumerable<IFunction> functions) : IFunctionEvaluator
{
    private readonly IEnumerable<IFunction> _functions = functions;

    public Result<object?> Evaluate(FunctionCall functionCall, IExpressionEvaluator expressionEvaluator, object? context)
        => functionCall switch
        {
            null => Result.Invalid<object?>("Function call is required"),
            _ => _functions
            .Select(x => x.Evaluate(functionCall, context, this, expressionEvaluator))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.NotSupported<object?>($"Unknown function: {functionCall.FunctionName}")
        };

    public Result Validate(FunctionCall functionCall, IExpressionEvaluator expressionEvaluator, object? context)
        => functionCall switch
        {
            null => Result.Invalid("Function call is required"),
            _ => _functions
            .Select(x => x.Validate(functionCall, context, this, expressionEvaluator))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Invalid($"Unknown function: {functionCall.FunctionName}")
        };
}
