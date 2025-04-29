namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class DateDotExpressionComponent : IDotExpressionComponent
{
    public int Order => 11;

    public Result<object?> Evaluate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Type == DotExpressionType.Property && state.Part == "Date" && state.CurrentEvaluateResult.Value is DateTime dateTime)
        {
            return Result.Success<object?>(dateTime.Date);
        }

        return Result.Continue<object?>();
    }

    public Result<Type> Validate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Type == DotExpressionType.Property && state.Part == "Date" && state.ResultType == typeof(DateTime))
        {
            return Result.Success(typeof(DateTime));
        }

        return Result.Continue<Type>();
    }
}
