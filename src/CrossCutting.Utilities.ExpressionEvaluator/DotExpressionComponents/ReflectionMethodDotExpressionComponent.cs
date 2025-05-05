namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class ReflectionMethodDotExpressionComponent : IDotExpressionComponent
{
    public int Order => 102;

    public Result<object?> Evaluate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (!state.Context.Settings.AllowReflection || state.Type != DotExpressionType.Method)
        {
            return Result.Continue<object?>();
        }

        var functionCall = state.FunctionParseResult.GetValueOrThrow();
        var methods = state.Value.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Where(x => x.Name == functionCall.Name && x.GetParameters().Length == functionCall.Arguments.Count)
            .ToArray();

        if (methods.Length == 0)
        {
            return Result.Invalid<object?>($"Type {state.Value.GetType().FullName} does not contain method {functionCall.Name}");
        }
        else if (methods.Length > 1)
        {
            return Result.Invalid<object?>($"Method {functionCall.Name} on type {state.Value.GetType().FullName} has multiple overloads with {functionCall.Arguments.Count} arguments, this is not supported");
        }

        var args = functionCall.Arguments
            .Select(x => state.Context.Evaluate(x))
            .TakeWhileWithFirstNonMatching(x => x.IsSuccessful())
            .ToArray();

        if (args.Length > 0 && !args[args.Length - 1].IsSuccessful())
        {
            return args[args.Length - 1];
        }

        state.AppendPart();

        return Result.WrapException(() => Result.Success<object?>(methods[0].Invoke(state.Value, args.Select(x => x.Value).ToArray())));
    }

    public Result<Type> Validate(DotExpressionComponentState state)
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
            return Result.Invalid<Type>($"Type {state.ResultType!.FullName} does not contain method {functionCall.Name}");
        }
        else if (methods.Length > 1)
        {
            return Result.Invalid<Type>($"Method {functionCall.Name} on type {state.ResultType!.FullName} has multiple overloads with {functionCall.Arguments.Count} arguments, this is not supported");
        }

        return Result.Success(methods[0].ReturnType);
    }
}
