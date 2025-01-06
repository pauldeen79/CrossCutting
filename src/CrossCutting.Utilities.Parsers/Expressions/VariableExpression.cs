namespace CrossCutting.Utilities.Parsers.Expressions;

public class VariableExpression : IExpression
{
    public int Order => 50;

    private readonly IVariableProcessor _variableProcessor;

    public VariableExpression(IVariableProcessor variableProcessor)
    {
        ArgumentGuard.IsNotNull(variableProcessor, nameof(variableProcessor));

        _variableProcessor = variableProcessor;
    }

    public Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context)
    {
        if (expression is null)
        {
            return Result.Continue<object?>();
        }

        if (expression.StartsWith("$") && expression.Length > 1)
        {
            return _variableProcessor.Evaluate(expression.Substring(1), context);
        }

        return Result.Continue<object?>();
    }

    public Result Validate(string expression, IFormatProvider formatProvider, object? context)
    {
        if (expression is null)
        {
            return Result.Continue();
        }

        if (expression.StartsWith("$") && expression.Length > 1)
        {
            return _variableProcessor.Validate(expression.Substring(1), context);
        }

        // Other values are ignored, so the expression parser knows whether an expression is supported
        return Result.Continue();
    }
}
