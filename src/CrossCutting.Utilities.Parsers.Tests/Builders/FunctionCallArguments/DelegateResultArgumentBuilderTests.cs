namespace CrossCutting.Utilities.Parsers.Tests.Builders.FunctionCallArguments;

public class DelegateResultArgumentBuilderTests
{
    [Fact]
    public void BuildTyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgumentBuilder<string>().WithDelegate(() => Result.Success("Some value"));

        // Act
        var result = sut.BuildTyped();

        // Assert
        result.ShouldBeOfType<DelegateResultArgument<string>>();
        result.Delegate().ShouldBe(sut.Delegate());
    }

    [Fact]
    public void Build_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgumentBuilder<string>().WithDelegate(() => Result.Success("Some value"));

        // Act
        var result = sut.Build();

        // Assert
        result.ShouldBeOfType<DelegateResultArgument<string>>();
        ((DelegateResultArgument<string>)result).Delegate().Value.ShouldBe(sut.Delegate().Value);
    }

    [Fact]
    public void Result_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgumentBuilder<string>().WithDelegate(() => Result.Success("Some value"));

        // Act
        var result = sut.Delegate();

        // Assert
        result.Value.ShouldBe("Some value");
    }

    [Fact]
    public void WithDelegate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgumentBuilder<string>().WithDelegate(() => Result.Success("Some value"));

        // Act
        var result = sut.WithDelegate(() => Result.Success("Altered value"));

        // Assert
        result.ShouldBeSameAs(sut);
        result.Delegate().Value.ShouldBe("Altered value");
    }

    [Fact]
    public void WithValidationDelegate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgumentBuilder<string>().WithDelegate(() => Result.Success("Some value"));

        // Act
        var result = sut.WithValidationDelegate(() => Result.Success(typeof(string)));

        // Assert
        result.ShouldBeSameAs(sut);
        result.ValidationDelegate.ShouldNotBeNull();
        result.ValidationDelegate!().Value.ShouldNotBeNull();
        result.ValidationDelegate().Value.ShouldBe(typeof(string));
    }
}
