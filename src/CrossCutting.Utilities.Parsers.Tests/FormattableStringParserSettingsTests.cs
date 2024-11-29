namespace CrossCutting.Utilities.Parsers.Tests;

public class FormattableStringParserSettingsTests
{
    public class Ctor : FormattableStringParserSettingsTests
    {
        [Fact]
        public void Throws_On_PlaceholderStart_With_Unsupported_Character()
        {
            // Arrange
            var builder = new FormattableStringParserSettingsBuilder();

            // Act & Assert
            builder.WithPlaceholderStart("\uE002").Invoking(x => x.Build()).Should().Throw<ValidationException>().WithMessage("PlaceholderStart cannot contain the \uE002 character");
        }

        [Fact]
        public void Throws_On_PlaceholderEnd_With_Unsupported_Character()
        {
            // Arrange
            var builder = new FormattableStringParserSettingsBuilder();

            // Act & Assert
            builder.WithPlaceholderEnd("\uE002").Invoking(x => x.Build()).Should().Throw<ValidationException>().WithMessage("PlaceholderEnd cannot contain the \uE002 character");
        }

        [Fact]
        public void Throws_On_PlaceholderStart_Same_As_PlaceholderEnd()
        {
            // Arrange
            var builder = new FormattableStringParserSettingsBuilder();

            // Act & Assert
            builder.WithPlaceholderStart("x").WithPlaceholderEnd("x").Invoking(x => x.Build()).Should().Throw<ValidationException>().WithMessage("PlaceholderStart and PlaceholderEnd cannot have the same value");
        }
    }
}
