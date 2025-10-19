namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class ReflectionMethodDotExpressionComponent : IDotExpressionComponent
{
    public int Order => 102;

    public async Task<Result<object?>> EvaluateAsync(DotExpressionComponentState state, CancellationToken token)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (!state.Context.Settings.AllowReflection || state.Type != DotExpressionType.Method)
        {
            return Result.Continue<object?>();
        }

        var result = state.FunctionParseResult.EnsureNotNull().EnsureValue();
        if (!result.IsSuccessful())
        {
            return result;
        }

        var functionCall = state.FunctionParseResult.Value!;
        var methods = state.Value.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Where(x => x.Name == functionCall.Name && x.GetParameters().Length == functionCall.Arguments.Count)
            .ToArray();

        if (methods.Length == 0)
        {
            var methodsByName = state.Value.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.Name == functionCall.Name)
                .ToArray();
            return Result.Invalid<object?>(methodsByName.Length == 0
                ? $"Type {state.Value.GetType().FullName} does not contain method {functionCall.Name}"
                : $"Type {state.Value.GetType().FullName} does not contain method {functionCall.Name} with {functionCall.Arguments.Count} arguments");
        }
        else if (methods.Length > 1)
        {
            return Result.Invalid<object?>($"Method {functionCall.Name} on type {state.Value.GetType().FullName} has multiple overloads with {functionCall.Arguments.Count} arguments, this is not supported");
        }

        var args = new List<Result<object?>>();
        foreach (var argument in functionCall.Arguments)
        {
            var argumentResult = await state.Context.EvaluateAsync(argument, token).ConfigureAwait(false);
            args.Add(argumentResult);
            if (!argumentResult.IsSuccessful())
            {
                break;
            }
        }

        if (args.Count > 0 && !args[args.Count - 1].IsSuccessful())
        {
            return args[args.Count - 1];
        }

        state.AppendPart();

        return Result.WrapException(() => Result.Success<object?>(methods[0].Invoke(state.Value, args.Select(x => x.Value).ToArray())));
    }

    public Task<Result<Type>> ValidateAsync(DotExpressionComponentState state, CancellationToken token)
        => Task.Run(() =>
        {
            state = ArgumentGuard.IsNotNull(state, nameof(state));

            if (!state.Context.Settings.AllowReflection || state.Type != DotExpressionType.Method)
            {
                return Result.Continue<Type>();
            }

            var functionCall = state.FunctionParseResult.GetValueOrThrow();
            var methods = state.ResultType!
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.Name == functionCall.Name && x.GetParameters().Length == functionCall.Arguments.Count)
                .ToArray();

            if (methods.Length == 0)
            {
                var methodsByName = state.ResultType!
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.Name == functionCall.Name)
                    .ToArray();
                return Result.Invalid<Type>(methodsByName.Length == 0
                    ? $"Type {state.ResultType!.FullName} does not contain method {functionCall.Name}"
                    : $"Type {state.ResultType!.FullName} does not contain method {functionCall.Name} with {functionCall.Arguments.Count} arguments");
            }
            else if (methods.Length > 1)
            {
                return Result.Invalid<Type>($"Method {functionCall.Name} on type {state.ResultType!.FullName} has multiple overloads with {functionCall.Arguments.Count} arguments, this is not supported");
            }

            return Result.Success(methods[0].ReturnType);
        }, token);
}
