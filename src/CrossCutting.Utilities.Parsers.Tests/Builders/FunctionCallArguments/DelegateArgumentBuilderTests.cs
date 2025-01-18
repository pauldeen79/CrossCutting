namespace CrossCutting.Utilities.Parsers.Tests.Builders.FunctionCallArguments;

public class DelegateArgumentBuilderTests
{
    [Fact]
    public void ToUntyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateArgumentBuilder<string>().WithDelegate(() => "Some value");

        // Act
        var result = sut.ToUntyped();

        // Assert
        result.Should().BeOfType<DelegateArgumentBuilder>();
        ((DelegateArgumentBuilder)result).Delegate().Should().Be(sut.Delegate());
    }

    [Fact]
    public void BuildTyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateArgumentBuilder<string>().WithDelegate(() => "Some value");

        // Act
        var result = sut.BuildTyped();

        // Assert
        result.Should().BeOfType<DelegateArgument<string>>();
        ((DelegateArgument<string>)result).Delegate().Should().Be(sut.Delegate());
    }

    [Fact]
    public void Build_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateArgumentBuilder<string>().WithDelegate(() => "Some value");

        // Act
        var result = sut.Build();

        // Assert
        result.Should().BeOfType<DelegateArgument>();
        ((DelegateArgument)result).Delegate().Should().Be(sut.Delegate());
    }

    [Fact]
    public void Value_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateArgumentBuilder<string>().WithDelegate(() => "Some value");

        // Act
        var result = sut.Delegate();

        // Assert
        result.Should().Be("Some value");
    }

    [Fact]
    public void WithDelegate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateArgumentBuilder<string>().WithDelegate(() => "Some value");

        // Act
        var result = sut.WithDelegate(() => "Altered value");

        // Assert
        result.Should().BeSameAs(sut);
        result.Delegate().Should().Be("Altered value");
    }
}
