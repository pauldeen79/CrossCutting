namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public abstract class TestBase
{
    protected IExpressionEvaluator Evaluator { get; }
    protected IExpression Expression { get; }

    protected ExpressionEvaluatorContext CreateContext(string? expression, object? context = null, int currentRecursionLevel = 1, ExpressionEvaluatorContext? parentContext = null)
        => new ExpressionEvaluatorContext(expression, new ExpressionEvaluatorSettingsBuilder(), context, Evaluator, currentRecursionLevel, parentContext);

    protected TestBase()
    {
        // Initialize evaluator
        Evaluator = Substitute.For<IExpressionEvaluator>();
        Evaluator
            .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
            .Returns(Evaluate);
        Evaluator
            .Parse(Arg.Any<ExpressionEvaluatorContext>())
            .Returns(x => Evaluate(x).Transform(result =>
                new ExpressionParseResultBuilder()
                    .WithSourceExpression(x.ArgAt<ExpressionEvaluatorContext>(0).Expression)
                    .WithExpressionType(typeof(TestBase))
                    .WithResultType(result.Value?.GetType())
                    .WithStatus(result.Status)
                    .WithErrorMessage(result.ErrorMessage)
                    .AddValidationErrors(result.ValidationErrors)
            ));

        // Initialize expression
        Expression = Substitute.For<IExpression>();
        // Note that you have to setup Evaluate and Validate method yourself
    }

    // Test stub for expression evaluation, that supports strings, integers and booleans (last two by using TryParse) as well as the context keyword
    private static Result<object?> Evaluate(CallInfo callInfo)
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

        if (context.Expression.StartsWith('"') && context.Expression.StartsWith('"'))
        {
            return Result.Success<object?>(context.Expression.Substring(1, context.Expression.Length - 2));
        }

        if (int.TryParse(context.Expression, context.Settings.FormatProvider, out int number))
        {
            return Result.Success<object?>(number);
        }

        if (bool.TryParse(context.Expression, out bool boolean))
        {
            return Result.Success<object?>(boolean);
        }

        return Result.NotSupported<object?>($"Unsupported expression: {context.Expression}");
    }
}

public abstract class TestBase<T> : TestBase where T : new()
{
    protected static T CreateSut() => new T();
}
