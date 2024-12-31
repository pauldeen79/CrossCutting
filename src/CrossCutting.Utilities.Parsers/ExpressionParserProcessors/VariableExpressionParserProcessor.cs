namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class VariableExpressionParserProcessor : IExpression
{
    public int Order => 50;

    private readonly IVariableProcessor _variableProcessor;

    public VariableExpressionParserProcessor(IVariableProcessor variableProcessor)
    {
        ArgumentGuard.IsNotNull(variableProcessor, nameof(variableProcessor));

        _variableProcessor = variableProcessor;
    }

    public Result<object?> Evaluate(string value, IFormatProvider formatProvider, object? context)
    {
        if (value is null)
        {
            return Result.Continue<object?>();
        }

        if (value.StartsWith("$") && value.Length > 1)
        {
            return _variableProcessor.Evaluate(value.Substring(1), context);
        }

        return Result.Continue<object?>();
    }

    public Result Validate(string value, IFormatProvider formatProvider, object? context)
    {
        if (value is null)
        {
            return Result.Continue();
        }

        if (value.StartsWith("$") && value.Length > 1)
        {
            return _variableProcessor.Validate(value.Substring(1), context);
        }

        // Other values are ignored, so the expression parser knows whether an expression is supported
        return Result.Continue();
    }
}
