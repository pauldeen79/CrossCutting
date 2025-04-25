namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class PropertyExpressionComponent : IExpressionComponent
{
    public int Order => 30;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var split = context.Expression.SplitDelimited('.', '"', leaveTextQualifier: true, trimItems: true);
        if (split.Length <= 1)
        {
            return Result.Continue<object?>();
        }

        var result = context.Evaluate(split[0]);
        if (!result.IsSuccessful())
        {
            return result;
        }

        var currentExpression = new StringBuilder(split[0]);

        foreach (var propertyName in split.Skip(1))
        {
            if (result.Value is null)
            {
                return Result.Invalid<object?>($"{currentExpression} is null, cannot get property {propertyName}");
            }

            var property = result.Value.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            if (property is null)
            {
                return Result.Invalid<object?>($"Type {result.Value.GetType().FullName} does not contain property {propertyName}");
            }

            var value = property.GetValue(result.Value);
            currentExpression.Append('.').Append(propertyName);
            result = Result.Success<object?>(value);
        }

        return result;
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithExpressionComponentType(typeof(PropertyExpressionComponent))
            .WithSourceExpression(context.Expression);

        var split = context.Expression.SplitDelimited('.', '"', leaveTextQualifier: true, trimItems: true);
        if (split.Length <= 1)
        {
            return result.WithStatus(ResultStatus.Continue);
        }

        var firstResult = context.Parse(split[0]);
        if (!firstResult.IsSuccessful())
        {
            return firstResult.ToBuilder().WithExpressionComponentType(typeof(PropertyExpressionComponent));
        }

        return result.WithStatus(ResultStatus.Ok);
    }
}
