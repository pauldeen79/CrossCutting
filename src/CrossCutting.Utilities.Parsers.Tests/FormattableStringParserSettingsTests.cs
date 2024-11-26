namespace CrossCutting.Utilities.Parsers.Tests;

public class FormattableStringParserSettingsTests
{
    public class Ctor : FormattableStringParserSettingsTests
    {
        [Fact]
        public void Throws_On_Invalid_PlaceholderStart()
        {
            // Arrange
            var builder = new FormattableStringParserSettingsBuilder();

            // Act & Assert
            builder.WithPlaceholderStart("{[").Invoking(x => x.Build()).Should().Throw<ValidationException>();
        }

        [Fact]
        public void Throws_On_Invalid_PlaceholderEnd()
        {
            // Arrange
            var builder = new FormattableStringParserSettingsBuilder();

            // Act & Assert
            builder.WithPlaceholderEnd("{[").Invoking(x => x.Build()).Should().Throw<ValidationException>();
        }
    }
}
