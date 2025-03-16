namespace CrossCutting.Utilities.Parsers.Expressions;

public class DateTimeExpression : IExpression
{
    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Expression switch
        {
            not null when DateTime.TryParse(context.Expression, context.Settings.FormatProvider, DateTimeStyles.None, out var dt) => Result.Success<object?>(dt),
            _ => Result.Continue<object?>()
        };
    }

    public Result<Type> Validate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Expression switch
        {
            not null when DateTime.TryParse(context.Expression, context.Settings.FormatProvider, DateTimeStyles.None, out _) => Result.Success(typeof(DateTime)),
            _ => Result.Continue<Type>()
        };
    }
}
