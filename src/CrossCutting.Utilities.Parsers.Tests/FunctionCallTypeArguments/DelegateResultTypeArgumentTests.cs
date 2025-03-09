namespace CrossCutting.Utilities.Parsers.Tests.FunctionCallTypeArguments;

public class DelegateResultTypeArgumentTests
{
    private static FunctionEvaluatorSettings CreateSettings()
        => new FunctionEvaluatorSettingsBuilder();

    [Fact]
    public void Evaluate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultTypeArgument(() => Result.Success(typeof(string)), null);
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CreateSettings(), null);

        // Act
        var result = sut.Evaluate(context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(typeof(string));
    }

    [Fact]
    public void Validate_Returns_Correct_Result_Without_Custom_Result()
    {
        // Arrange
        var sut = new DelegateResultTypeArgument(() => Result.Success(typeof(string)), null);
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CreateSettings(), null);

        // Act
        var result = sut.Validate(context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(typeof(Type));
    }

    [Fact]
    public void Validate_Returns_Correct_Result_With_Custom_Result()
    {
        // Arrange
        var sut = new DelegateResultTypeArgument(() => Result.Success(typeof(string)), () => Result.Success(typeof(string)));
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CreateSettings(), null);

        // Act
        var result = sut.Validate(context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(typeof(string));
    }

    [Fact]
    public void ToBuilder_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultTypeArgument(() => Result.Success(typeof(string)), null);

        // Act
        var result = sut.ToBuilder();

        // Assert
        result.ShouldBeOfType<DelegateResultTypeArgumentBuilder>();
        ((DelegateResultTypeArgumentBuilder)result).Delegate.ShouldBeSameAs(sut.Delegate);
    }

    [Fact]
    public void ToTypedBuilder_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultTypeArgument(() => Result.Success(typeof(string)), null);

        // Act
        var result = sut.ToTypedBuilder();

        // Assert
        result.ShouldBeOfType<DelegateResultTypeArgumentBuilder>();
        result.Delegate.ShouldBeSameAs(sut.Delegate);
    }

    [Fact]
    public void Delegate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultTypeArgument(() => Result.Success(typeof(string)), null);

        // Act
        var result = sut.Delegate();

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(typeof(string));
    }
    
    [Fact]
    public void IsDynamic_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultTypeArgument(() => Result.Success(typeof(string)), null);

        // Act
        var result = sut.IsDynamic;

        // Assert
        result.ShouldBeTrue();
    }
}
