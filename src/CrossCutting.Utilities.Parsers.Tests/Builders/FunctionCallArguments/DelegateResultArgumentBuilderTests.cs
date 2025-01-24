namespace CrossCutting.Utilities.Parsers.Tests.Builders.FunctionCallArguments;

public class DelegateResultArgumentBuilderTests
{
    [Fact]
    public void ToUntyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgumentBuilder<string>().WithDelegate(() => Result.Success("Some value"));

        // Act
        var result = sut.ToUntyped();

        // Assert
        result.Should().BeOfType<DelegateResultArgumentBuilder>();
        ((DelegateResultArgumentBuilder)result).Delegate().Value.Should().Be(sut.Delegate().Value);
    }

    [Fact]
    public void BuildTyped_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgumentBuilder<string>().WithDelegate(() => Result.Success("Some value"));

        // Act
        var result = sut.BuildTyped();

        // Assert
        result.Should().BeOfType<DelegateResultArgument<string>>();
        ((DelegateResultArgument<string>)result).Delegate().Should().Be(sut.Delegate());
    }

    [Fact]
    public void Build_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgumentBuilder<string>().WithDelegate(() => Result.Success("Some value"));

        // Act
        var result = sut.Build();

        // Assert
        result.Should().BeOfType<DelegateResultArgument>();
        ((DelegateResultArgument)result).Delegate().Value.Should().Be(sut.Delegate().Value);
    }

    [Fact]
    public void Result_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgumentBuilder<string>().WithDelegate(() => Result.Success("Some value"));

        // Act
        var result = sut.Delegate();

        // Assert
        result.Value.Should().Be("Some value");
    }

    [Fact]
    public void WithDelegate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgumentBuilder<string>().WithDelegate(() => Result.Success("Some value"));

        // Act
        var result = sut.WithDelegate(() => Result.Success("Altered value"));

        // Assert
        result.Should().BeSameAs(sut);
        result.Delegate().Value.Should().Be("Altered value");
    }


    [Fact]
    public void WithValidationDelegate_Returns_Correct_Result()
    {
        // Arrange
        var sut = new DelegateResultArgumentBuilder<string>().WithDelegate(() => Result.Success("Some value"));

        // Act
        var result = sut.WithValidationDelegate(() => Result.Success(typeof(string)));

        // Assert
        result.Should().BeSameAs(sut);
        result.ValidationDelegate.Should().NotBeNull();
        result.ValidationDelegate!().Value.Should().NotBeNull();
        result.ValidationDelegate().Value.Should().Be<string>();
    }
}
