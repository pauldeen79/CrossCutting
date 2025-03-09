namespace CrossCutting.Utilities.Parsers.Tests.FunctionCallTypeArguments;

public class ConstantTypeArgumentTests
{
    private static FunctionEvaluatorSettings CreateSettings()
        => new FunctionEvaluatorSettingsBuilder();

    [Fact]
    public void Evaluate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantTypeArgument(typeof(string));
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CreateSettings(), null);

        // Act
        var result = sut.Evaluate(context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(typeof(string));
    }

    [Fact]
    public void Validate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantTypeArgument(typeof(string));
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CreateSettings(), null);

        // Act
        var result = sut.Validate(context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(typeof(Type));
    }

    [Fact]
    public void ToBuilder_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantTypeArgument(typeof(string));

        // Act
        var result = sut.ToBuilder();

        // Assert
        result.ShouldBeOfType<ConstantTypeArgumentBuilder>();
        ((ConstantTypeArgumentBuilder)result).Value.ShouldBe(sut.Value);
    }

    [Fact]
    public void ToTypedBuilder_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantTypeArgument(typeof(string));

        // Act
        var result = sut.ToTypedBuilder();

        // Assert
        result.ShouldBeOfType<ConstantTypeArgumentBuilder>();
        result.Value.ShouldBe(sut.Value);
    }

    [Fact]
    public void Value_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantTypeArgument(typeof(string));

        // Act
        var result = sut.Value;

        // Assert
        result.ShouldBe(typeof(string));
    }
    
    [Fact]
    public void IsDynamic_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantTypeArgument(typeof(string));

        // Act
        var result = sut.IsDynamic;

        // Assert
        result.ShouldBeFalse();
    }
}
