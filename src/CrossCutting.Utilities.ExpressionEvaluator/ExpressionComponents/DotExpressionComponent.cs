namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class DotExpressionComponent : IExpressionComponent
{
    private readonly Regex _propertyNameRegEx = new Regex("^[A-Za-z]+$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
    private readonly IFunctionParser _functionParser;

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

        var currentExpression = new StringBuilder(split[0]);

        foreach (var part in split.Skip(1))
        {
            if (result.Value is null)
            {
                return Result.Invalid<object?>($"{currentExpression} is null, cannot get property {part}");
            }

            if (_propertyNameRegEx.IsMatch(part))
            {
                var property = result.Value.GetType().GetProperty(part, BindingFlags.Instance | BindingFlags.Public);
                if (property is null)
                {
                    return Result.Invalid<object?>($"Type {result.Value.GetType().FullName} does not contain property {part}");
                }

                var value = property.GetValue(result.Value);
                currentExpression.Append('.').Append(part);
                result = Result.Success<object?>(value);
                continue;
            }

            var functionParseResult = _functionParser.Parse(context.CreateChildContext(part));
            if (functionParseResult.IsSuccessful())
            {
                var functionCall = functionParseResult.GetValueOrThrow();
                var methods = result.Value.GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.Name == functionCall.Name && x.GetParameters().Length == functionCall.Arguments.Count)
                    .ToArray();

                if (methods.Length == 0)
                {
                    return Result.Invalid<object?>($"Type {result.Value.GetType().FullName} does not contain method {part}");
                }
                else if (methods.Length > 1)
                {
                    return Result.Invalid<object?>($"Method {part} on type {result.Value.GetType().FullName} has multiple overloads with {functionCall.Arguments.Count} arguments, this is not supported");
                }

                var args = functionCall.Arguments
                    .Select(x => context.Evaluate(x))
                    .TakeWhileWithFirstNonMatching(x => x.IsSuccessful())
                    .ToArray();
                if (args.Length > 0 && !args[args.Length - 1].IsSuccessful())
                {
                    return args[args.Length - 1];
                }

                result = Result.Success<object?>(methods[0].Invoke(result.Value, args.Select(x => x.Value).ToArray()));
                continue;
            }

            return Result.Invalid<object?>($"Unrecognized expression: {part}");
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

        return result.WithStatus(ResultStatus.Ok);
    }
}
