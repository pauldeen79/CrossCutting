namespace CrossCutting.Utilities.Parsers.Tests.FunctionCallArguments;

public class ConstantResultArgumentTests
{
    [Fact]
    public void EvaluateTyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgument<string>(Result.Success("Hello world!"));
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.EvaluateTyped(context);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("Hello world!");
    }

    [Fact]
    public void Evaluate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgument<string>(Result.Success("Hello world!"));
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.Evaluate(context);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("Hello world!");
    }

    [Fact]
    public void Validate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgument<string>(Result.Success("Hello world!"));
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.Validate(context);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be<string>();
    }

    [Fact]
    public void ToBuilder_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgument<string>(Result.Success("Hello world!"));
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.ToBuilder();

        // Assert
        result.Should().BeOfType<ConstantResultArgumentBuilder<string>>();
        ((ConstantResultArgumentBuilder<string>)result).Result.Should().Be(sut.Result);
    }

    [Fact]
    public void ToTypedBuilder_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgument<string>(Result.Success("Hello world!"));
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.ToTypedBuilder();

        // Assert
        result.Should().BeOfType<ConstantResultArgumentBuilder<string>>();
        ((ConstantResultArgumentBuilder<string>)result).Result.Should().Be(sut.Result);
    }

    [Fact]
    public void Result_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgument<string>(Result.Success("Hello world!"));
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.Result;

        // Assert
        result.Value.Should().Be("Hello world!");
    }
}
