namespace CrossCutting.Utilities.Parsers.Tests.Builders.FunctionCallArguments;

public class ConstantArgumentBuilderTests
{
    [Fact]
    public void BuildTyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantArgumentBuilder<string>().WithValue("Some value");

        // Act
        var result = sut.BuildTyped();

        // Assert
        result.ShouldBeOfType<ConstantArgument<string>>();
        result.Value.ShouldBe(sut.Value);
    }

    [Fact]
    public void Build_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantArgumentBuilder<string>().WithValue("Some value");

        // Act
        var result = sut.Build();

        // Assert
        result.ShouldBeOfType<ConstantArgument<string>>();
        ((ConstantArgument<string>)result).Value.ShouldBe(sut.Value);
    }

    [Fact]
    public void Value_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantArgumentBuilder<string>().WithValue("Some value");

        // Act
        var result = sut.Value;

        // Assert
        result.ShouldBe("Some value");
    }

    [Fact]
    public void WithValue_Returns_Correct_Result()
    {
        // Arrange
        var sut = new ConstantArgumentBuilder<string>().WithValue("Some value");

        // Act
        var result = sut.WithValue("Altered value");

        // Assert
        result.ShouldBeSameAs(sut);
        result.Value.ShouldBe("Altered value");
    }
}
