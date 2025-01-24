namespace CrossCutting.Utilities.Parsers.Tests.FunctionCallArguments;

public class DelegateArgumentTests
{
    [Fact]
    public void EvaluateTyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateArgument<string>(() => "Hello world!", null);
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
        var sut = new DelegateArgument<string>(() => "Hello world!", null);
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.Evaluate(context);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("Hello world!");
    }

    [Fact]
    public void Untyped_Evaluate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateArgument(() => "Hello world!", null);
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
        var sut = new DelegateArgument<string>(() => "Hello world!", () => typeof(string));
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.Validate(context);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be<string>();
    }

    [Fact]
    public void Untyped_Validate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateArgument(() => "Hello world!", () => typeof(string));
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
        var sut = new DelegateArgument<string>(() => "Hello world!", null);

        // Act
        var result = sut.ToBuilder();

        // Assert
        result.Should().BeOfType<DelegateArgumentBuilder<string>>();
        ((DelegateArgumentBuilder<string>)result).Delegate().Should().Be(sut.Delegate());
    }

    [Fact]
    public void ToTypedBuilder_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateArgument<string>(() => "Hello world!", null);

        // Act
        var result = sut.ToTypedBuilder();

        // Assert
        result.Should().BeOfType<DelegateArgumentBuilder<string>>();
        ((DelegateArgumentBuilder<string>)result).Delegate().Should().Be(sut.Delegate());
    }

    [Fact]
    public void Delegate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateArgument<string>(() => "Hello world!", null);

        // Act
        var result = sut.Delegate();

        // Assert
        result.Should().Be("Hello world!");
    }
}
