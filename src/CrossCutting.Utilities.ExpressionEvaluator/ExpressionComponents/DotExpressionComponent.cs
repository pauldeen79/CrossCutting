namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class DotExpressionComponent : IExpressionComponent
{
    private readonly IFunctionParser _functionParser;
    private readonly IDotExpressionComponent[] _components;

    public int Order => 50;

    public DotExpressionComponent(IFunctionParser functionParser, IEnumerable<IDotExpressionComponent> components)
    {
        components = ArgumentGuard.IsNotNull(components, nameof(components));
        ArgumentGuard.IsNotNull(functionParser, nameof(functionParser));

        _functionParser = functionParser;
        _components = components.OrderBy(x => x.Order).ToArray();
    }

    public async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var split = context.Expression.SplitDelimited('.', '"', leaveTextQualifier: true, trimItems: true);
        if (split.Length <= 1)
        {
            return Result.Continue<object?>();
        }

        return await (await context.EvaluateAsync(split[0], token).ConfigureAwait(false))
            .OnSuccessAsync(async result =>
            {
                var state = new DotExpressionComponentState(context, _functionParser, result, split[0]);

                var (flowControl, value) = await ProcessState(split, state, token).ConfigureAwait(false);
                if (!flowControl)
                {
                    return value;
                }

                return state.CurrentEvaluateResult;
            }).ConfigureAwait(false);
    }

    public async Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token)
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

        var firstResult = await context.ParseAsync(split[0], token).ConfigureAwait(false);
        if (!firstResult.IsSuccessful())
        {
            return firstResult.ToBuilder().WithExpressionComponentType(typeof(DotExpressionComponent));
        }

        var state = new DotExpressionComponentState(context, _functionParser, firstResult.ToResult(), split[0]);
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

            foreach (var component in _components)
            {
                state.CurrentParseResult = await component.ValidateAsync(state, token).ConfigureAwait(false);
                if (state.CurrentParseResult.Status != ResultStatus.Continue)
                {
                    break;
                }
            }

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

    private async Task<(bool flowControl, Result<object?> value)> ProcessState(string[] split, DotExpressionComponentState state, CancellationToken token)
    {
        foreach (var part in split.Skip(1))
        {
            state.Part = part;

            if (state.CurrentEvaluateResult.Value is null)
            {
                return (flowControl: false, value: Result.Invalid<object?>($"{state.CurrentExpression} is null, cannot evaluate {state.TypeDisplayName} {state.Name}"));
            }

            state.Value = state.CurrentEvaluateResult.Value;

            foreach (var component in _components)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                state.CurrentEvaluateResult = await component.EvaluateAsync(state, token).ConfigureAwait(false);
                if (state.CurrentEvaluateResult.Status != ResultStatus.Continue)
                {
                    break;
                }
            }

            if (!state.CurrentEvaluateResult.IsSuccessful())
            {
                return (flowControl: false, value: state.CurrentEvaluateResult);
            }
            else if (state.CurrentEvaluateResult.Status == ResultStatus.Continue)
            {
                return (flowControl: false, value: Result.Invalid<object?>($"Unknown {state.TypeDisplayName} on type {(state.ResultType ?? typeof(object)).FullName}: {state.Name}"));
            }
        }

        return (flowControl: true, value: Result.NoContent<object?>());
    }
}
