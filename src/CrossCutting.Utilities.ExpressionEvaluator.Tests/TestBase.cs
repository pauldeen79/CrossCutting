namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public abstract class TestBase
{
    protected IExpressionEvaluator Evaluator { get; }
    protected IExpression Expression { get; }

    protected ExpressionEvaluatorContext CreateContext(string expression)
        => new ExpressionEvaluatorContext(expression, new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(CultureInfo.InvariantCulture), null, Evaluator);

    protected TestBase()
    {
        // Initialize evaluator
        Evaluator = Substitute.For<IExpressionEvaluator>();
        Evaluator
            .Evaluate(Arg.Any<string>(), Arg.Any<ExpressionEvaluatorSettings>(), Arg.Any<object?>())
            .Returns(Evaluate);
        Evaluator
            .Validate (Arg.Any<string>(), Arg.Any<ExpressionEvaluatorSettings>(), Arg.Any<object?>())
            .Returns(x => Evaluate(x).Transform(_ => typeof(bool)));

        // Initialize expression
        Expression = Substitute.For<IExpression>();
        // Note that you have to setup Evaluate and Validate method yourself
    }

    // Test stub for expression evaluation, that supports strings, integers and booleans (last two by using TryParse) as well as the context keyword
    private static Result<object?> Evaluate(CallInfo callInfo)
    {
        var expression = callInfo.ArgAt<string>(0);
        var settings = callInfo.ArgAt<ExpressionEvaluatorSettings>(1);

        if (expression == "context")
        {
            return Result.Success(callInfo.ArgAt<object?>(2));
        }

        if (expression == "context.Length")
        {
            return Result.Success<object?>((callInfo.ArgAt<object?>(2)?.ToString() ?? string.Empty).Length);
        }

        if (expression == "error")
        {
            return Result.Error<object?>("Kaboom");
        }

        if (expression.StartsWith('"') && expression.StartsWith('"'))
        {
            return Result.Success<object?>(expression.Substring(1, expression.Length - 2));
        }

        if (int.TryParse(expression, settings.FormatProvider, out int number))
        {
            return Result.Success<object?>(number);
        }

        if (bool.TryParse(expression, out bool boolean))
        {
            return Result.Success<object?>(boolean);
        }

        return Result.NotSupported<object?>($"Unsupported expression: {expression}");
    }
}

public abstract class TestBase<T> : TestBase where T : new()
{
    protected static T CreateSut() => new T();
}
