namespace CrossCutting.Utilities.Parsers.Tests.Builders.FunctionCallArguments;

public class ConstantResultArgumentBuilderTests
{
    [Fact]
    public void BuildTyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgumentBuilder<string>().WithResult(Result.Success("Some value"));

        // Act
        var result = sut.BuildTyped();

        // Assert
        result.ShouldBeOfType<ConstantResultArgument<string>>();
        result.Result.ShouldBe(sut.Result);
    }

    [Fact]
    public void Build_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgumentBuilder<string>().WithResult(Result.Success("Some value"));

        // Act
        var result = sut.Build();

        // Assert
        result.ShouldBeOfType<ConstantResultArgument<string>>();
        ((ConstantResultArgument<string>)result).Result.Value.ShouldBe(sut.Result.Value);
    }

    [Fact]
    public void Result_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgumentBuilder<string>().WithResult(Result.Success("Some value"));

        // Act
        var result = sut.Result;

        // Assert
        result.Value.ShouldBe("Some value");
    }

    [Fact]
    public void WithResult_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgumentBuilder<string>().WithResult(Result.Success("Some value"));

        // Act
        var result = sut.WithResult(Result.Success("Altered value"));

        // Assert
        result.ShouldBeSameAs(sut);
        result.Result.Value.ShouldBe("Altered value");
    }
}
