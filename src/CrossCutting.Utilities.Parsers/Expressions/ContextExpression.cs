namespace CrossCutting.Utilities.Parsers.Expressions;

public class ContextExpression : IExpression
{
    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
        => context.IsNotNull(nameof(context)).Expression switch
        {
            "context" => Result.Success(context.Context),
            _ => Result.Continue<object?>()
        };

    public Result<Type> Validate(ExpressionEvaluatorContext context)
        => context.IsNotNull(nameof(context)).Expression switch
        {
            "context" => Result.Success(context?.GetType()!),
            _ => Result.Continue<Type>()
        };
}
