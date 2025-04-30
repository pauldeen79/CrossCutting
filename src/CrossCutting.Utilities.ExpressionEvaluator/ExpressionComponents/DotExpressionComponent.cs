namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class DotExpressionComponent : IExpressionComponent
{
    private readonly IFunctionParser _functionParser;
    private readonly IDotExpressionComponent[] _components;

    public int Order => 30;

    public DotExpressionComponent(IFunctionParser functionParser, IEnumerable<IDotExpressionComponent> components)
    {
        components = ArgumentGuard.IsNotNull(components, nameof(components));
        ArgumentGuard.IsNotNull(functionParser, nameof(functionParser));

        _functionParser = functionParser;
        _components = components.OrderBy(x => x.Order).ToArray();
    }

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

        var state = new DotExpressionComponentState(context, _functionParser, result, split[0]);

        foreach (var part in split.Skip(1))
        {
            state.Part = part;

            if (state.CurrentEvaluateResult.Value is null)
            {
                return Result.Invalid<object?>($"{state.CurrentExpression} is null, cannot evaluate {state.TypeDisplayName} {state.Name}");
            }

            state.Value = state.CurrentEvaluateResult.Value;

            state.CurrentEvaluateResult = _components
                .Select(x => x.Evaluate(state))
                .TakeWhileWithFirstNonMatching(x => x.Status == ResultStatus.Continue)
                .Last();

            if (!state.CurrentEvaluateResult.IsSuccessful())
            {
                return state.CurrentEvaluateResult;
            }
            else if (state.CurrentEvaluateResult.Status == ResultStatus.Continue)
            {
                return Result.Invalid<object?>($"Unknown {state.TypeDisplayName} on type {(state.ResultType ?? typeof(object)).FullName}: {state.Name}");
            }
        }

        return state.CurrentEvaluateResult;
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

        var state = new DotExpressionComponentState(context, _functionParser, firstResult.ToResult().TryCastAllowNull<object?>(), split[0]);
        state.ResultType = firstResult.ResultType;

        foreach (var part in split.Skip(1))
        {
            state.Part = part;

            if (state.ResultType is null)
            {
                return result
                    .WithStatus(ResultStatus.Invalid)
                    .WithErrorMessage($"{state.CurrentExpression} is null, cannot evaluate {state.TypeDisplayName} {state.Name}");
            }

            state.CurrentParseResult = _components
                .Select(x => x.Validate(state))
                .TakeWhileWithFirstNonMatching(x => x.Status == ResultStatus.Continue)
                .Last();

            if (!state.CurrentParseResult.IsSuccessful())
            {
                return result.FillFromResult(state.CurrentParseResult);
            }
            else if (state.CurrentParseResult.Status == ResultStatus.Continue)
            {
                return result
                    .WithStatus(ResultStatus.Invalid)
                    .WithErrorMessage($"Unknown {state.TypeDisplayName}: {state.Name}");
            }

            state.ResultType = state.CurrentParseResult.Value;
        }

        return result
            .WithStatus(ResultStatus.Ok)
            .WithResultType(state.ResultType);
    }
}
