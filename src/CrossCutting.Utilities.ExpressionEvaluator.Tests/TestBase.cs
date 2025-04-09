namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public abstract class TestBase
{
    protected IExpressionEvaluator Evaluator { get; }
    protected IExpression Expression { get; }

    protected ExpressionEvaluatorContext CreateContext(string? expression, object? context = null, int currentRecursionLevel = 1, ExpressionEvaluatorContext? parentContext = null, IExpressionEvaluator? evaluator = null, bool validateArgumentTypes = true)
        => new ExpressionEvaluatorContext(expression, new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes(validateArgumentTypes), context, evaluator ?? Evaluator, currentRecursionLevel, parentContext);

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
                    "error" => new ExpressionParseResultBuilder().WithExpressionType(GetType()).WithStatus(ResultStatus.Error).WithErrorMessage("Kaboom"),
                    "123" => new ExpressionParseResultBuilder().WithExpressionType(GetType()).WithStatus(ResultStatus.Ok).WithResultType(typeof(int)),
                    "string" => new ExpressionParseResultBuilder().WithExpressionType(GetType()).WithStatus(ResultStatus.Ok).WithResultType(typeof(string)),
                    _ => new ExpressionParseResultBuilder().WithExpressionType(GetType()).WithStatus(ResultStatus.Ok)
                });

        // Initialize expression
        Expression = Substitute.For<IExpression>();
        // Note that you have to setup Evaluate and Validate method yourself
    }

    // Test stub for expression evaluation, that supports strings, integers, long integers, decimals, booleans and DeteTimes (by using TryParse), as well as the context and null keywords
    protected static Result<object?> EvaluateExpression(CallInfo callInfo)
    {
        var context = callInfo.ArgAt<ExpressionEvaluatorContext>(0);

        if (context.Expression == "null")
        {
            return Result.Success(default(object?));
        }

        if (context.Expression == "context")
        {
            return Result.Success(context.Context);
        }

        if (context.Expression == "context.Length")
        {
            return Result.Success<object?>((context.Context?.ToString() ?? string.Empty).Length);
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

public abstract class TestBase<T> : TestBase where T : new()
{
    protected static T CreateSut() => new T();
}
