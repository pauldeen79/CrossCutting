namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class DotExpressionComponent : IExpressionComponent
{
    private static readonly Regex _propertyNameRegEx = new Regex("^[A-Za-z]+$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
    private readonly IFunctionParser _functionParser;

    private static readonly Func<DotExpressionComponentState, Result<object?>>[] _evaluateProcessors =
        [
            EvaluateProperty,
            EvaluateMethod
        ];

    private static readonly Func<DotExpressionComponentState, Result<Type>>[] _validateProcessors =
        [
            ValidateProperty,
            ValidateMethod
        ];

    public int Order => 30;

    public DotExpressionComponent(IFunctionParser functionParser)
    {
        ArgumentGuard.IsNotNull(functionParser, nameof(functionParser));

        _functionParser = functionParser;
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

        var state = new DotExpressionComponentState(context, _functionParser, split[0]);

        foreach (var part in split.Skip(1))
        {
            state.Part = part;

            if (result.Value is null)
            {
                return Result.Invalid<object?>($"{state.CurrentExpression} is null, cannot get property or method {state.Part}");
            }

            state.Value = result.Value;

            result = _evaluateProcessors
                .Select(x => x.Invoke(state))
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

        if (!context.Settings.AllowReflection)
        {
            return result.WithStatus(ResultStatus.Continue);
        }

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

        var state = new DotExpressionComponentState(context, _functionParser, split[0]);
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

            var subResult = _validateProcessors
                .Select(x => x.Invoke(state))
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

    private static Result<object?> EvaluateProperty(DotExpressionComponentState state)
    {
        if (!_propertyNameRegEx.IsMatch(state.Part))
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

    private static Result<object?> EvaluateMethod(DotExpressionComponentState state)
    {
        var functionParseResult = state.FunctionParser.Parse(state.Context.CreateChildContext(state.Part));
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

    private static Result<Type> ValidateProperty(DotExpressionComponentState state)
    {
        if (!_propertyNameRegEx.IsMatch(state.Part))
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

    private static Result<Type> ValidateMethod(DotExpressionComponentState state)
    {
        var functionParseResult = state.FunctionParser.Parse(state.Context.CreateChildContext(state.Part));
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
