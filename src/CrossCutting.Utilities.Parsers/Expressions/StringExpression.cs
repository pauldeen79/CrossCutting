namespace CrossCutting.Utilities.Parsers.Expressions;

public class StringExpression : IExpression
{
    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Expression?.StartsWith("\"") switch
        {
            true when context.Expression.EndsWith("\"") => Result.Success<object?>(context.Expression.Substring(1, context.Expression.Length - 2)),
            _ => Result.Continue<object?>()
        };
    }

    public Result<Type> Validate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Expression?.StartsWith("\"") switch
        {
            true when context.Expression.EndsWith("\"") => Result.Success(typeof(string)),
            _ => Result.Continue<Type>()
        };
    }
}
