using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace CrossCutting.Utilities.Parsers.Tests
{
    [ExcludeFromCodeCoverage]
    public class FlatTextParserTests
    {
        [Fact]
        public void Can_Parse_FlatText_Without_TextQualifier()
        {
            // Arrange
            var input = "Value A\tValue B\tValue C";

            // Act
            var actual = FlatTextParser.Parse(input, '\t');

            // Assert
            actual.Should().HaveCount(3);
            actual.Should().HaveElementAt(0, "Value A");
            actual.Should().HaveElementAt(1, "Value B");
            actual.Should().HaveElementAt(2, "Value C");
        }

        [Fact]
        public void Can_Parse_FlatText_With_TextQualifier()
        {
            // Arrange
            var input = "\"Value A\" \"Value B\" \"Value C\"";

            // Act
            var actual = FlatTextParser.Parse(input, ' ', '"');

            // Assert
            actual.Should().HaveCount(3);
            actual.Should().HaveElementAt(0, "Value A");
            actual.Should().HaveElementAt(1, "Value B");
            actual.Should().HaveElementAt(2, "Value C");
        }
    }
}
