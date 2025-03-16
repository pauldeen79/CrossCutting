namespace CrossCutting.Utilities.Parsers.Expressions;

public class VariableExpression : IExpression
{
    private readonly IVariableEvaluator _variableProcessor;

    public VariableExpression(IVariableEvaluator variableProcessor)
    {
        ArgumentGuard.IsNotNull(variableProcessor, nameof(variableProcessor));

        _variableProcessor = variableProcessor;
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Expression.StartsWith("$") switch
        {
            true when context.Expression.Length > 1 => _variableProcessor.Evaluate(context.Expression.Substring(1), context.Context),
            _ => Result.Continue<object?>()
        };
    }

    public Result<Type> Validate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Expression.StartsWith("$") switch
        {
            true when context.Expression.Length > 1 => _variableProcessor.Validate(context.Expression.Substring(1), context.Context),
            _ => Result.Continue<Type>()
        };
    }
}
