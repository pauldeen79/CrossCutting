namespace CrossCutting.Utilities.Parsers.Tests.Builders.FunctionCallArguments;

public class ConstantArgumentBuilderTests
{
    [Fact]
    public void ToUntyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantArgumentBuilder<string>().WithValue("Some value");

        // Act
        var result = sut.ToUntyped();

        // Assert
        result.Should().BeOfType<ConstantArgumentBuilder>();
        ((ConstantArgumentBuilder)result).Value.Should().Be(sut.Value);
    }

    [Fact]
    public void BuildTyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantArgumentBuilder<string>().WithValue("Some value");

        // Act
        var result = sut.BuildTyped();

        // Assert
        result.Should().BeOfType<ConstantArgument<string>>();
        ((ConstantArgument<string>)result).Value.Should().Be(sut.Value);
    }

    [Fact]
    public void Build_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantArgumentBuilder<string>().WithValue("Some value");

        // Act
        var result = sut.Build();

        // Assert
        result.Should().BeOfType<ConstantArgument>();
        ((ConstantArgument)result).Value.Should().Be(sut.Value);
    }

    [Fact]
    public void Value_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantArgumentBuilder<string>().WithValue("Some value");

        // Act
        var result = sut.Value;

        // Assert
        result.Should().Be("Some value");
    }

    [Fact]
    public void WithValue_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantArgumentBuilder<string>().WithValue("Some value");

        // Act
        var result = sut.WithValue("Altered value");

        // Assert
        result.Should().BeSameAs(sut);
        result.Value.Should().Be("Altered value");
    }
}
