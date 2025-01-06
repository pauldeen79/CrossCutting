namespace CrossCutting.Utilities.Parsers.Tests;

public class ExpressionFrameworkNewTests
{
    [Fact]
    public void Can_Validate_ToLowerCaseExpression()
    {
        // Arrange
        var sut = new ToLowerCaseFunction();
        var functionEvaluator = Substitute.For<IFunctionEvaluator>();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(x => Result.Success<object?>(x.ArgAt<string>(0)));
        var functionCall = new FunctionCallBuilder().WithName("ToUpperCase").AddArguments(new ConstantArgumentBuilder().WithValue("Hello world!")).Build();
        var request = new FunctionCallContext(functionCall, functionEvaluator, expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.Validate(request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Evaluate_ToLowerCaseExpression()
    {
        // Arrange
        var sut = new ToLowerCaseFunction();
        var functionEvaluator = Substitute.For<IFunctionEvaluator>();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(x => Result.Success<object?>(x.ArgAt<string>(0)));
        var functionCall = new FunctionCallBuilder().WithName("ToLowerCase").AddArguments(new ConstantArgumentBuilder().WithValue("Hello world!")).Build();
        var request = new FunctionCallContext(functionCall, functionEvaluator, expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.Evaluate(request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeOfType<string>();
        result.Value!.ToString().Should().Be("hello world!");
    }

    [Fact]
    public void Can_Get_FunctionDescriptor()
    {
        // Arrange
        var functionDescriptorProvider = new FunctionDescriptorProvider([new ToLowerCaseFunction()]);

        // Act
        var functionDescriptors = functionDescriptorProvider.GetAll();

        // Assert
        functionDescriptors.Should().ContainSingle();
        functionDescriptors.Single().Arguments.Should().ContainSingle();
        functionDescriptors.Single().Results.Should().HaveCount(3);
    }
}

[FunctionName(@"ToLowerCase")]
[Description("Converts the expression to lower case")]
[FunctionArgument(nameof(Expression), typeof(string), "String to get the lower case for", true)]
[FunctionResult(ResultStatus.Ok, typeof(string), "The value of the expression converted to lower case", "This result will be returned when the expression is of type string")]
[FunctionResult(ResultStatus.Invalid, "Expression must be of type string")]
[FunctionResult(ResultStatus.Invalid, "CultureInfo must be of type CultureInfo")]
public class ToLowerCaseFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext request)
    {
        request = ArgumentGuard.IsNotNull(request, nameof(request));

        var expressionResult = request.FunctionCall.Arguments.First().GetValueResult(request).TryCast<string>();
        if (!expressionResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(expressionResult);
        }

        var cultureInfoResult = request.FunctionCall.Arguments.ElementAtOrDefault(1)?.GetValueResult(request).TryCast<CultureInfo>();
        if (cultureInfoResult is not null && !cultureInfoResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(cultureInfoResult);
        }

#pragma warning disable CA1308 // Normalize strings to uppercase
        return Result.Success<object?>(cultureInfoResult is null
            ? expressionResult.Value!.ToLowerInvariant()
            : expressionResult.Value!.ToLower(cultureInfoResult.Value!));
#pragma warning restore CA1308 // Normalize strings to uppercase
    }

    public Result Validate(FunctionCallContext request)
    {
        // No additional validation needed
        return Result.Success();
    }
}

public record ToLowerCaseExpression : ITypedExpression<string>
{
    public ToLowerCaseExpression(ITypedExpression<string> expression)
    {
        Expression = expression;
    }

    public ITypedExpression<string> Expression { get; }

    public Result<string> EvaluateTyped(object? context)
    {
        // not needed anymore
        throw new NotImplementedException();
    }

    public Expression ToUntyped()
    {
        throw new NotImplementedException();
    }
}

public class ToLowerCaseExpressionBuilder : ITypedExpressionBuilder<string>
{
    public ToLowerCaseExpressionBuilder()
    {
        Expression = new TypedConstantExpressionBuilder<string>();
    }

    public ITypedExpressionBuilder<string> Expression { get; set; }

    public ITypedExpression<string> Build()
    {
        return new ToLowerCaseExpression(Expression.Build());
    }
}
