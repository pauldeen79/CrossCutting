namespace CrossCutting.Utilities.Parsers.Tests.Builders.FunctionCallArguments;

public class DelegateArgumentBuilderTests
{
    [Fact]
    public void BuildTyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateArgumentBuilder<string>().WithDelegate(() => "Some value");

        // Act
        var result = sut.BuildTyped();

        // Assert
        result.ShouldBeOfType<DelegateArgument<string>>();
        result.Delegate().ShouldBe(sut.Delegate());
    }

    [Fact]
    public void Build_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateArgumentBuilder<string>().WithDelegate(() => "Some value");

        // Act
        var result = sut.Build();

        // Assert
        result.ShouldBeOfType<DelegateArgument<string>>();
        ((DelegateArgument<string>)result).Delegate().ShouldBe(sut.Delegate());
    }

    [Fact]
    public void Value_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateArgumentBuilder<string>().WithDelegate(() => "Some value");

        // Act
        var result = sut.Delegate();

        // Assert
        result.ShouldBe("Some value");
    }

    [Fact]
    public void WithDelegate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateArgumentBuilder<string>().WithDelegate(() => "Some value");

        // Act
        var result = sut.WithDelegate(() => "Altered value");

        // Assert
        result.ShouldBeSameAs(sut);
        result.Delegate().ShouldBe("Altered value");
    }

    [Fact]
    public void WithValidationDelegate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateArgumentBuilder<string>().WithDelegate(() => "Some value");

        // Act
        var result = sut.WithValidationDelegate(() => typeof(string));

        // Assert
        result.ShouldBeSameAs(sut);
        result.ValidationDelegate.ShouldNotBeNull();
        result.ValidationDelegate!().ShouldBe(typeof(string));
    }
}
