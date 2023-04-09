namespace CrossCutting.Common.Tests.Extensions;

public class StringExtensionsTests
{
    [Theory,
        InlineData("", "replaced"),
        InlineData(null, "replaced"),
        InlineData("other", "other"),
        InlineData(" ", " ")]
    public void WhenNullOrEmpty_Returns_Correct_Value(string? input, string expectedOutput)
    {
        // Act
        var actual_noDelegate = input.WhenNullOrEmpty("replaced");
        var actual_delegate = input.WhenNullOrEmpty(() => "replaced");

        // Assert
        actual_noDelegate.Should().Be(expectedOutput);
        actual_delegate.Should().Be(expectedOutput);
    }

    [Theory,
        InlineData("", "replaced"),
        InlineData(null, "replaced"),
        InlineData("other", "other"),
        InlineData(" ", "replaced")]
    public void WhenNullOrWhitespace_Returns_Correct_Value(string? input, string expectedOutput)
    {
        // Act
        var actual_noDelegate = input.WhenNullOrWhitespace("replaced");
        var actual_delegate = input.WhenNullOrWhitespace(() => "replaced");

        // Assert
        actual_noDelegate.Should().Be(expectedOutput);
        actual_delegate.Should().Be(expectedOutput);
    }

    [Theory,
        InlineData("y", true),
        InlineData("Y", true),
        InlineData("yes", true),
        InlineData("1", true),
        InlineData("other value", false),
        InlineData(null, false)]
    public void IsTrue_Returns_Correct_Result(string input, bool expectedResult)
    {
        // Act
        var actual = input.IsTrue();

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Theory,
        InlineData("n", true),
        InlineData("N", true),
        InlineData("no", true),
        InlineData("0", true),
        InlineData("other value", false),
        InlineData(null, false)]
    public void IsFalse_Returns_Correct_Result(string input, bool expectedResult)
    {
        // Act
        var actual = input.IsFalse();

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Fact]
    public void StartsWithAny_Returns_True_When_Found()
    {
        // Arrange
        var input = "Axx";

        // Act
        var actual_array = input.StartsWithAny("A", "B", "C");
        var actual_enumerable = input.StartsWithAny(new List<string> { "A", "B", "C" });

        // Assert
        actual_array.Should().BeTrue();
        actual_enumerable.Should().BeTrue();
    }

    [Fact]
    public void StartsWithAny_Returns_False_When_Not_Found()
    {
        // Arrange
        var input = "Dxx";

        // Act
        var actual = input.StartsWithAny("A", "B", "C");

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void StartsWithAny_StringComparisonOverload_Returns_True_When_Found()
    {
        // Arrange
        var input = "Axx";

        // Act
        var actual_array = input.StartsWithAny(StringComparison.InvariantCultureIgnoreCase, "A", "b", "C");
        var actual_enumerable = input.StartsWithAny(StringComparison.InvariantCultureIgnoreCase, new List<string> { "A", "B", "c" });

        // Assert
        actual_array.Should().BeTrue();
        actual_enumerable.Should().BeTrue();
    }

    [Fact]
    public void StartsWithAny_StringComparisonOverload_Returns_False_When_Not_Found()
    {
        // Arrange
        var input = "Dxx";

        // Act
        var actual_array = input.StartsWithAny(StringComparison.InvariantCultureIgnoreCase, "A", "b", "C");
        var actual_enumerable = input.StartsWithAny(StringComparison.InvariantCultureIgnoreCase, new List<string> { "A", "B", "c" });

        // Assert
        actual_array.Should().BeFalse();
        actual_enumerable.Should().BeFalse();
    }

    [Fact]
    public void EndsWithAny_Returns_True_When_Found()
    {
        // Arrange
        var input = "xxA";

        // Act
        var actual_array = input.EndsWithAny("A", "B", "C");
        var actual_enumerable = input.EndsWithAny(new List<string> { "A", "B", "C" });

        // Assert
        actual_array.Should().BeTrue();
        actual_enumerable.Should().BeTrue();
    }

    [Fact]
    public void EndsWithAny_Returns_False_When_Not_Found()
    {
        // Arrange
        var input = "xxD";

        // Act
        var actual_array = input.EndsWithAny("A", "B", "C");
        var actual_enumerable = input.EndsWithAny(new List<string> { "A", "B", "C" });

        // Assert
        actual_array.Should().BeFalse();
        actual_enumerable.Should().BeFalse();
    }

    [Fact]
    public void EndsWithAny_StringComparisonOverload_Returns_True_When_Found()
    {
        // Arrange
        var input = "xxA";

        // Act
        var actual_array = input.EndsWithAny(StringComparison.InvariantCultureIgnoreCase, "A", "b", "C");
        var actual_enumerable = input.EndsWithAny(StringComparison.InvariantCultureIgnoreCase, new List<string> { "A", "B", "c" });

        // Assert
        actual_array.Should().BeTrue();
        actual_enumerable.Should().BeTrue();
    }

    [Fact]
    public void EndsWithAny_StringComparisonOverload_Returns_False_When_Not_Found()
    {
        // Arrange
        var input = "xxD";

        // Act
        var actual_array = input.EndsWithAny(StringComparison.InvariantCultureIgnoreCase, "A", "b", "C");
        var actual_enumerable = input.EndsWithAny(StringComparison.InvariantCultureIgnoreCase, new List<string> { "A", "B", "c" });

        // Assert
        actual_array.Should().BeFalse();
        actual_enumerable.Should().BeFalse();
    }

    [Fact]
    public void NormalizeLineEndings_Returns_Correct_Result()
    {
        // Arrange
        const string input = "some\r\nmore\ndata";

        // Act
        var result = input.NormalizeLineEndings();

        // Assert
        result.Should().Be($"some{Environment.NewLine}more{Environment.NewLine}data");
    }
}
