﻿namespace CrossCutting.Utilities.Parsers.Tests.FunctionCallArguments;

public class ConstantResultArgumentTests
{
    private static FunctionEvaluatorSettings CreateSettings()
        => new FunctionEvaluatorSettingsBuilder();

    [Fact]
    public void EvaluateTyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgument<string>(Result.Success("Hello world!"));
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
        var sut = new ConstantResultArgument<string>(Result.Success("Hello world!"));
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
        var sut = new ConstantResultArgument(Result.Success<object?>("Hello world!"));
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
        var sut = new ConstantResultArgument<string>(Result.Success("Hello world!"));
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
        var sut = new ConstantResultArgument(Result.Success<object?>("Hello world!"));
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
        var sut = new ConstantResultArgument<string>(Result.Success("Hello world!"));

        // Act
        var result = sut.ToBuilder();

        // Assert
        result.ShouldBeOfType<ConstantResultArgumentBuilder<string>>();
        ((ConstantResultArgumentBuilder<string>)result).Result.ShouldBe(sut.Result);
    }

    [Fact]
    public void ToTypedBuilder_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgument<string>(Result.Success("Hello world!"));

        // Act
        var result = sut.ToTypedBuilder();

        // Assert
        result.ShouldBeOfType<ConstantResultArgumentBuilder<string>>();
        result.Result.ShouldBe(sut.Result);
    }

    [Fact]
    public void Result_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgument<string>(Result.Success("Hello world!"));

        // Act
        var result = sut.Result;

        // Assert
        result.Value.ShouldBe("Hello world!");
    }

    [Fact]
    public void IsDynamic_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgument<string>(Result.Success("Hello world!"));

        // Act
        var result = sut.IsDynamic;

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void Untyped_IsDynamic_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgument(Result.Success<object?>("Hello world!"));

        // Act
        var result = sut.IsDynamic;

        // Assert
        result.ShouldBeFalse();
    }
}
