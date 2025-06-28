namespace CrossCutting.Utilities.QueryEvaluator.Expressions;

public partial record FieldNameExpression
{
    public override Result<object?> Evaluate(object? context)
    {
        if (context is null)
        {
            return Result.NoContent<object?>();
        }

        var property = context.GetType().GetProperty(FieldName, BindingFlags.Instance | BindingFlags.Public);
        if (property is null)
        {
            return Result.Invalid<object?>($"Type {context.GetType().FullName} does not contain property {FieldName}");
        }

        return Result.WrapException(() =>
        {
            var propertyValue = property.GetValue(context);

            return Result.Success<object?>(propertyValue);
        });
    }
}
