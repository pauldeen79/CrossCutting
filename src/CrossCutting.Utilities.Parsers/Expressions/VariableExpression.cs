namespace CrossCutting.Utilities.Parsers.Expressions;

public class VariableExpression : IExpression
{
    private readonly IVariableProcessor _variableProcessor;

    public VariableExpression(IVariableProcessor variableProcessor)
    {
        ArgumentGuard.IsNotNull(variableProcessor, nameof(variableProcessor));

        _variableProcessor = variableProcessor;
    }

    public Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context)
        => expression?.StartsWith("$") switch
        {
            true when expression.Length > 1 => _variableProcessor.Evaluate(expression.Substring(1), context),
            _ => Result.Continue<object?>()
        };

    public Result<Type> Validate(string expression, IFormatProvider formatProvider, object? context)
        => expression?.StartsWith("$") switch
        {
            true when expression.Length > 1 => _variableProcessor.Validate(expression.Substring(1), context),
            _ => Result.Continue<Type>()
        };
}
