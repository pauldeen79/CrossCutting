namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class DotExpressionComponent : IExpressionComponent
{
    private readonly IDotExpressionComponent[] _components;

    public int Order => 30;

    public DotExpressionComponent(IEnumerable<IDotExpressionComponent> components)
    {
        components = ArgumentGuard.IsNotNull(components, nameof(components));

        _components = components.OrderBy(x => x.Order).ToArray();
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (!context.Settings.AllowReflection)
        {
            return Result.Continue<object?>();
        }

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

        var state = new DotExpressionComponentState(context, split[0]);

        foreach (var part in split.Skip(1))
        {
            state.Part = part;

            if (result.Value is null)
            {
                return Result.Invalid<object?>($"{state.CurrentExpression} is null, cannot get property or method {state.Part}");
            }

            state.Value = result.Value;

            result = _components
                .Select(x => x.Evaluate(state))
                .TakeWhileWithFirstNonMatching(x => x.Status == ResultStatus.Continue)
                .Last();

            if (!result.IsSuccessful())
            {
                return result;
            }
            else if (result.Status == ResultStatus.Continue)
            {
                return Result.Invalid<object?>($"Unrecognized expression: {state.Part}");
            }
        }

        return result;
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithExpressionComponentType(typeof(DotExpressionComponent))
            .WithSourceExpression(context.Expression);

        var split = context.Expression.SplitDelimited('.', '"', leaveTextQualifier: true, trimItems: true);
        if (split.Length <= 1)
        {
            return result.WithStatus(ResultStatus.Continue);
        }

        var firstResult = context.Parse(split[0]);
        if (!firstResult.IsSuccessful())
        {
            return firstResult.ToBuilder().WithExpressionComponentType(typeof(DotExpressionComponent));
        }

        var state = new DotExpressionComponentState(context, split[0]);
        state.ResultType = firstResult.ResultType;

        foreach (var part in split.Skip(1))
        {
            state.Part = part;

            if (state.ResultType is null)
            {
                return result
                    .WithStatus(ResultStatus.Invalid)
                    .WithErrorMessage($"{state.CurrentExpression} is null, cannot get property or method {state.Part}");
            }

            var subResult = _components
                .Select(x => x.Validate(state))
                .TakeWhileWithFirstNonMatching(x => x.Status == ResultStatus.Continue)
                .Last();

            if (!subResult.IsSuccessful())
            {
                return result.FillFromResult(subResult);
            }
            else if (subResult.Status == ResultStatus.Continue)
            {
                return result
                    .WithStatus(ResultStatus.Invalid)
                    .WithErrorMessage($"Unrecognized expression: {state.Part}");
            }

            state.ResultType = subResult.Value;
        }

        return result.WithStatus(ResultStatus.Ok);
    }
}
