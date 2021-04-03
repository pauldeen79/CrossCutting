using CrossCutting.Common.Extensions;
using FluentAssertions;
using Xunit;

namespace CrossCutting.Common.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Theory,
            InlineData("", "replaced"),
            InlineData(null, "replaced"),
            InlineData("other", "other"),
            InlineData(" ", " ")]
        public void WhenNullOrEmpty_Returns_Correct_Value(string input, string expectedOutput)
        {
            // Act
            var actual = input.WhenNullOrEmpty("replaced");

            // Assert
            actual.Should().Be(expectedOutput);
        }

        [Theory,
            InlineData("", "replaced"),
            InlineData(null, "replaced"),
            InlineData("other", "other"),
            InlineData(" ", "replaced")]
        public void WhenNullOrWhitespace_Returns_Correct_Value(string input, string expectedOutput)
        {
            // Act
            var actual = input.WhenNullOrWhitespace("replaced");

            // Assert
            actual.Should().Be(expectedOutput);
        }

        [Theory,
            InlineData("y", true),
            InlineData("Y", true),
            InlineData("yes", true),
            InlineData("1", true),
            InlineData("other value", false)]
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
            InlineData("other value", false)]
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
            var actual = input.StartsWithAny("A", "B", "C");

            // Assert
            actual.Should().BeTrue();
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
        public void EndsWithAny_Returns_True_When_Found()
        {
            // Arrange
            var input = "xxA";

            // Act
            var actual = input.EndsWithAny("A", "B", "C");

            // Assert
            actual.Should().BeTrue();
        }


        [Fact]
        public void EndsWithAny_Returns_False_When_Not_Found()
        {
            // Arrange
            var input = "xxD";

            // Act
            var actual = input.EndsWithAny("A", "B", "C");

            // Assert
            actual.Should().BeFalse();
        }
    }
}
