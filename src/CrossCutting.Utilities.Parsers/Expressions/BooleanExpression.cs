namespace CrossCutting.Utilities.Parsers.Expressions;

public class BooleanExpression : IExpression
{
    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.IsNotNull(nameof(context)).Expression switch
        {
            "true" => true,
            "false" => false,
            _ => Result.Continue<object?>()
        };
    }

    public Result<Type> Validate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.IsNotNull(nameof(context)).Expression switch
        {
            "true" or "false" => typeof(bool),
            _ => Result.Continue<Type>()
        };
    }
}
