namespace CrossCutting.Utilities.Parsers.Expressions;

public class BooleanExpression : IExpression
{
    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.IsNotNull(nameof(context)).Expression switch
        {
            "true" => Result.Success<object?>(true),
            "false" => Result.Success<object?>(false),
            _ => Result.Continue<object?>()
        };
    }

    public Result<Type> Validate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.IsNotNull(nameof(context)).Expression switch
        {
            "true" => Result.Success(typeof(bool)),
            "false" => Result.Success(typeof(bool)),
            _ => Result.Continue<Type>()
        };
    }
}
