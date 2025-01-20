namespace CrossCutting.Utilities.Parsers.Tests.FunctionCallArguments;

public class ConstantArgumentTests
{
    [Fact]
    public void GetTypedValueResult_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantArgument<string>("Hello world!");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.GetTypedValueResult(context);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("Hello world!");
    }

    [Fact]
    public void GetValueResult_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantArgument<string>("Hello world!");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.Evaluate(context);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("Hello world!");
    }

    [Fact]
    public void ToBuilder_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantArgument<string>("Hello world!");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.ToBuilder();

        // Assert
        result.Should().BeOfType<ConstantArgumentBuilder<string>>();
        ((ConstantArgumentBuilder<string>)result).Value.Should().Be(sut.Value);
    }

    [Fact]
    public void ToTypedBuilder_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantArgument<string>("Hello world!");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.ToTypedBuilder();

        // Assert
        result.Should().BeOfType<ConstantArgumentBuilder<string>>();
        ((ConstantArgumentBuilder<string>)result).Value.Should().Be(sut.Value);
    }

    [Fact]
    public void Value_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantArgument<string>("Hello world!");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.Value;

        // Assert
        result.Should().Be("Hello world!");
    }
}
