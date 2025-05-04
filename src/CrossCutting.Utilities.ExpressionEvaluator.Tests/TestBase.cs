namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public abstract class TestBase
{
    protected IExpressionEvaluator Evaluator { get; }
    protected IExpressionComponent Expression { get; }
    protected IDateTimeProvider DateTimeProvider { get; }
    protected DateTime CurrentDateTime { get; }

    protected ExpressionEvaluatorContext CreateContext(
        string? expression,
        object? state,
        int currentRecursionLevel = 1,
        ExpressionEvaluatorContext? parentContext = null,
        IExpressionEvaluator? evaluator = null,
        ExpressionEvaluatorSettingsBuilder? settings = null)
    {
        IReadOnlyDictionary<string, Func<Result<object?>>>? dict = null;
        if (state is not null)
        {
            dict = new Dictionary<string, Func<Result<object?>>>
            {
                { "state", () => Result.Success<object?>(state) }
            };
        }

        return CreateContext(expression, dict, currentRecursionLevel, parentContext, evaluator, settings);
    }

    protected ExpressionEvaluatorContext CreateContext(
        string? expression,
        IReadOnlyDictionary<string, Func<Result<object?>>>? context = null,
        int currentRecursionLevel = 1,
        ExpressionEvaluatorContext? parentContext = null,
        IExpressionEvaluator? evaluator = null,
        ExpressionEvaluatorSettingsBuilder? settings = null)
            => new ExpressionEvaluatorContext(expression, settings ?? new ExpressionEvaluatorSettingsBuilder(), evaluator ?? Evaluator, context, currentRecursionLevel, parentContext);

    protected TestBase()
    {
        // Initialize evaluator
        Evaluator = Substitute.For<IExpressionEvaluator>();
        Evaluator
            .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
            .Returns(EvaluateExpression);
        Evaluator
            .Parse(Arg.Any<ExpressionEvaluatorContext>())
            .Returns(x =>
                x.ArgAt<ExpressionEvaluatorContext>(0).Expression switch
                {
                    "error" => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Error).WithErrorMessage("Kaboom"),
                    "-1" or "1" or "2" or "123" => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Ok).WithResultType(typeof(int)),
                    "string" => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Ok).WithResultType(typeof(string)),
                    "unknown" => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Ok),
                    "object" => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Ok).WithResultType(typeof(object)),
                    "state" => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Ok).WithResultType(x.ArgAt<ExpressionEvaluatorContext>(0).State?.FirstOrDefault().Value?.Invoke().Value?.GetType()),
                    _ => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Ok)
                });

        // Initialize expression
        Expression = Substitute.For<IExpressionComponent>();
        // Note that you have to setup Evaluate and Validate method yourself

        // Freeze DateTime.Now to a predicatable value
        DateTimeProvider = Substitute.For<IDateTimeProvider>();
        CurrentDateTime =  new DateTime(2025, 2, 1, 5, 30, 0, DateTimeKind.Utc);
        DateTimeProvider.GetCurrentDateTime().Returns(CurrentDateTime);

    }

    // Test stub for expression evaluation, that supports strings, integers, long integers, decimals, booleans and DeteTimes (by using TryParse), as well as the context and null keywords
    protected static Result<object?> EvaluateExpression(CallInfo callInfo)
    {
        var context = callInfo.ArgAt<ExpressionEvaluatorContext>(0);

        if (context.Expression == "null")
        {
            return Result.Success(default(object?));
        }

        var success = context.State.TryGetValue(context.Expression, out var dlg);
        if (success)
        {
            return dlg!();
        }

        if (context.Expression == "state.Length")
        {
            return Result.Success<object?>((context.State?.ToString() ?? string.Empty).Length);
        }

        if (context.Expression == "error")
        {
            return Result.Error<object?>("Kaboom");
        }

        if (context.Expression == "recursiveplaceholder")
        {
            return Result.Success<object?>("{recurse}");
        }

        if (context.Expression == "recurse")
        {
            return Result.Success<object?>("recursive value");
        }

        if (context.Expression.StartsWith('"') && context.Expression.StartsWith('"'))
        {
            return Result.Success<object?>(context.Expression.Substring(1, context.Expression.Length - 2));
        }

        if (int.TryParse(context.Expression, context.Settings.FormatProvider, out int number))
        {
            return Result.Success<object?>(number);
        }

        if (context.Expression.EndsWith('L') && long.TryParse(context.Expression[..^1], context.Settings.FormatProvider, out long longNumber))
        {
            return Result.Success<object?>(longNumber);
        }

        if (context.Expression.EndsWith('M') && decimal.TryParse(context.Expression[..^1], context.Settings.FormatProvider, out decimal decimalNumber))
        {
            return Result.Success<object?>(decimalNumber);
        }

        if (context.Expression.StartsWith("System."))
        {
            return Result.Success<object?>(Type.GetType(context.Expression));
        }

        if (bool.TryParse(context.Expression, out bool boolean))
        {
            return Result.Success<object?>(boolean);
        }

        if (context.Expression.StartsWith('"') && context.Expression.EndsWith('"'))
        {
            return Result.Success<object?>(context.Expression.Substring(1, context.Expression.Length - 2));
        }

        if (DateTime.TryParse(context.Expression, context.Settings.FormatProvider, out DateTime dateTime))
        {
            return Result.Success<object?>(dateTime);
        }

        return Result.NotSupported<object?>($"Unsupported expression: {context.Expression}");
    }
}

public abstract class TestBase<T> : TestBase
{
    protected T CreateSut() => (T)Common.Testing.TypeExtensions.CreateInstance(typeof(T), ClassFactory(), p =>
    {
        if (p.ParameterType == typeof(IExpressionEvaluator)) return new ExpressionEvaluator(new ExpressionTokenizer(), new ExpressionParser(), [Expression]);
        if (p.ParameterType == typeof(IExpressionTokenizer)) return new ExpressionTokenizer();
        if (p.ParameterType == typeof(IExpressionParser)) return new ExpressionParser();
        if (p.ParameterType == typeof(IFunctionParser)) return new FunctionParser();
        if (p.ParameterType == typeof(IDateTimeProvider)) return DateTimeProvider;
        if (p.ParameterType == typeof(IExpressionComponent)) return Expression;
        if (p.ParameterType == typeof(IEnumerable<IExpressionComponent>)) return new IExpressionComponent[] { Expression };
        if (p.ParameterType == typeof(IEnumerable<IDotExpressionComponent>)) return new IDotExpressionComponent[] { new ReflectionMethodDotExpressionComponent(), new ReflectionPropertyDotExpressionComponent() };
        return null;
    }, null)!;

    protected static Func<Type, object?> ClassFactory()
        => t => t.CreateInstance(parameterType => Substitute.For([parameterType], []), null, null);
}
