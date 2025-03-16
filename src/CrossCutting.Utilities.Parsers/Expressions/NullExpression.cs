namespace CrossCutting.Utilities.Parsers.Expressions;

public class NullExpression : IExpression
{
    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Expression switch
        {
            "null" => Result.Success<object?>(null),
            _ => Result.Continue<object?>()
        };
    }

    public Result<Type> Validate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Expression switch
        {
            "null" => Result.NoContent<Type>(),
            _ => Result.Continue<Type>()
        };
    }
}
