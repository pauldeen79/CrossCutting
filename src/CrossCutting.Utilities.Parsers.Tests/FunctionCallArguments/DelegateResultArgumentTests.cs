namespace CrossCutting.Utilities.Parsers.Tests.FunctionCallArguments;

public class DelegateResultArgumentTests
{
    private static FunctionEvaluatorSettings CreateSettings()
        => new FunctionEvaluatorSettingsBuilder();

    [Fact]
    public void EvaluateTyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgument<string>(() => Result.Success("Hello world!"), null);
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CreateSettings(), null);

        // Act
        var result = sut.EvaluateTyped(context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("Hello world!");
    }

    [Fact]
    public void Evaluate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgument<string>(() => Result.Success("Hello world!"), null);
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CreateSettings(), null);

        // Act
        var result = sut.Evaluate(context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("Hello world!");
    }

    [Fact]
    public void Untyped_Evaluate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgument(() => Result.Success<object?>("Hello world!"), null);
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CreateSettings(), null);

        // Act
        var result = sut.Evaluate(context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("Hello world!");
    }

    [Fact]
    public void Validate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgument<string>(() => Result.Success("Hello world!"), () => Result.Success(typeof(string)));
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CreateSettings(), null);

        // Act
        var result = sut.Validate(context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(typeof(string));
    }

    [Fact]
    public void Untyped_Validate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgument(() => Result.Success<object?>("Hello world!"), () => Result.Success(typeof(string)));
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
        var sut = new DelegateResultArgument<string>(() => Result.Success("Hello world!"), null);

        // Act
        var result = sut.ToBuilder();

        // Assert
        result.ShouldBeOfType<DelegateResultArgumentBuilder<string>>();
        ((DelegateResultArgumentBuilder<string>)result).Delegate().ShouldBe(sut.Delegate());
    }

    [Fact]
    public void ToTypedBuilder_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgument<string>(() => Result.Success("Hello world!"), null);

        // Act
        var result = sut.ToTypedBuilder();

        // Assert
        result.ShouldBeOfType<DelegateResultArgumentBuilder<string>>();
        result.Delegate().ShouldBe(sut.Delegate());
    }

    [Fact]
    public void Delegate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgument<string>(() => Result.Success("Hello world!"), null);

        // Act
        var result = sut.Delegate();

        // Assert
        result.Value.ShouldBe("Hello world!");
    }

    [Fact]
    public void IsDynamic_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgument<string>(() => Result.Success("Hello world!"), null);

        // Act
        var result = sut.IsDynamic;

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void Untyped_IsDynamic_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgument(() => Result.Success<object?>("Hello world!"), null);

        // Act
        var result = sut.IsDynamic;

        // Assert
        result.ShouldBeTrue();
    }
}
