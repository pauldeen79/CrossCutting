namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class ReflectionPropertyDotExpressionComponent : IDotExpressionComponent
{
    public int Order => 101;

    public Result<object?> Evaluate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (!state.Context.Settings.AllowReflection || state.Type != DotExpressionType.Property)
        {
            return Result.Continue<object?>();
        }

        var property = state.Value.GetType().GetProperty(state.Part, BindingFlags.Instance | BindingFlags.Public);
        if (property is null)
        {
            return Result.Invalid<object?>($"Type {state.Value.GetType().FullName} does not contain property {state.Part}");
        }

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            var propertyValue = property.GetValue(state.Value);
            state.AppendPart();

            return Result.Success<object?>(propertyValue);
        }
        catch (Exception ex)
        {
            return Result.Error<object?>(ex, $"Evaluation of property {state.Part} on type {state.Value.GetType().FullName} threw an exception, see Exception property for more details");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    public Result<Type> Validate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (!state.Context.Settings.AllowReflection || state.Type != DotExpressionType.Property)
        {
            return Result.Continue<Type>();
        }

        var property = state.ResultType!.GetProperty(state.Part, BindingFlags.Instance | BindingFlags.Public);
        if (property is null)
        {
            return Result.Invalid<Type>($"Type {state.ResultType.FullName} does not contain property {state.Part}");
        }

        return Result.Success(property.PropertyType);
    }
}
