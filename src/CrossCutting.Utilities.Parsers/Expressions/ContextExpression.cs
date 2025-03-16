namespace CrossCutting.Utilities.Parsers.Expressions;

public class ContextExpression : IExpression
{
    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Expression switch
        {
            "context" => Result.Success(context.Context),
            _ => Result.Continue<object?>()
        };
    }

    public Result<Type> Validate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Expression switch
        {
            "context" => Result.Success(context.Context?.GetType()!),
            _ => Result.Continue<Type>()
        };
    }
}
