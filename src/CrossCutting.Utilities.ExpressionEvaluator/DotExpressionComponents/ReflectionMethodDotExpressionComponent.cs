namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class ReflectionMethodDotExpressionComponent : IDotExpressionComponent
{
    private readonly IFunctionParser _functionParser;

    public int Order => 101;

    public ReflectionMethodDotExpressionComponent(IFunctionParser functionParser)
    {
        ArgumentGuard.IsNotNull(functionParser, nameof(functionParser));

        _functionParser = functionParser;
    }

    public Result<object?> Evaluate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (!state.Context.Settings.AllowReflection)
        {
            return Result.Continue<object?>();
        }

        var functionParseResult = _functionParser.Parse(state.Context.CreateChildContext(state.Part));
        if (!functionParseResult.IsSuccessful())
        {
            return functionParseResult.Status == ResultStatus.NotFound
                ? Result.Continue<object?>()
                : Result.FromExistingResult<object?>(functionParseResult);
        }

        var functionCall = functionParseResult.GetValueOrThrow();
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

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            state.AppendPart();

            return Result.Success<object?>(methods[0].Invoke(state.Value, args.Select(x => x.Value).ToArray()));
        }
        catch (Exception ex)
        {
            return Result.Error<object?>(ex, $"Evaluation of method {functionCall.Name} on type {state.Value.GetType().FullName} threw an exception, see Exception property for more details");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    public Result<Type> Validate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (!state.Context.Settings.AllowReflection)
        {
            return Result.Continue<Type>();
        }

        var functionParseResult = _functionParser.Parse(state.Context.CreateChildContext(state.Part));
        if (!functionParseResult.IsSuccessful())
        {
            return functionParseResult.Status == ResultStatus.NotFound
                ? Result.Continue<Type>()
                : Result.FromExistingResult<Type>(functionParseResult);
        }

        var functionCall = functionParseResult.GetValueOrThrow();
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
