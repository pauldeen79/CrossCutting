namespace CrossCutting.Utilities.Parsers.Tests.Builders.FunctionCallArguments;

public class ConstantResultArgumentBuilderTests
{
    [Fact]
    public void ToUntyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgumentBuilder<string>().WithResult(Result.Success("Some value"));

        // Act
        var result = sut.ToUntyped();

        // Assert
        result.Should().BeOfType<ConstantResultArgumentBuilder>();
        ((ConstantResultArgumentBuilder)result).Result.Value.Should().Be(sut.Result.Value);
    }

    [Fact]
    public void BuildTyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgumentBuilder<string>().WithResult(Result.Success("Some value"));

        // Act
        var result = sut.BuildTyped();

        // Assert
        result.Should().BeOfType<ConstantResultArgument<string>>();
        ((ConstantResultArgument<string>)result).Result.Should().Be(sut.Result);
    }

    [Fact]
    public void Build_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgumentBuilder<string>().WithResult(Result.Success("Some value"));

        // Act
        var result = sut.Build();

        // Assert
        result.Should().BeOfType<ConstantResultArgument>();
        ((ConstantResultArgument)result).Result.Value.Should().Be(sut.Result.Value);
    }

    [Fact]
    public void Result_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgumentBuilder<string>().WithResult(Result.Success("Some value"));

        // Act
        var result = sut.Result;

        // Assert
        result.Value.Should().Be("Some value");
    }

    [Fact]
    public void WithResult_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantResultArgumentBuilder<string>().WithResult(Result.Success("Some value"));

        // Act
        var result = sut.WithResult(Result.Success("Altered value"));

        // Assert
        result.Should().BeSameAs(sut);
        result.Result.Value.Should().Be("Altered value");
    }
}
