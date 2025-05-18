namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class ReflectionPropertyDotExpressionComponent : IDotExpressionComponent
{
    public int Order => 101;

    public Task<Result<object?>> EvaluateAsync(DotExpressionComponentState state, CancellationToken token)
        => Task.Run(() =>
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

            return Result.WrapException(() =>
            {
                var propertyValue = property.GetValue(state.Value);
                state.AppendPart();

                return Result.Success<object?>(propertyValue);
            });
        }, token);

    public Task<Result<Type>> ValidateAsync(DotExpressionComponentState state, CancellationToken token)
        => Task.Run(() =>
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
        }, token);
}
