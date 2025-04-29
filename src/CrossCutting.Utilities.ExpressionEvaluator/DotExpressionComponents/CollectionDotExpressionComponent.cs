namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class CollectionDotExpressionComponent : IDotExpressionComponent
{
    public int Order => 12;

    public Result<object?> Evaluate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Type == DotExpressionType.Property && state.Part == "Count" && state.CurrentEvaluateResult.Value is not null && state.CurrentEvaluateResult.Value is ICollection collection)
        {
            return Result.Success<object?>(collection.Count);
        }

        return Result.Continue<object?>();
    }

    public Result<Type> Validate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Type == DotExpressionType.Property && state.Part == "Count" && state.ResultType is not null && typeof(ICollection).IsAssignableFrom(state.ResultType))
        {
            return Result.Success(typeof(int));
        }

        return Result.Continue<Type>();
    }
}
