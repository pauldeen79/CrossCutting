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
            builder.WithPlaceholderStart("\uE002");

            // Act & Assert
            Action a = () => builder.Build();
            a.ShouldThrow<ValidationException>()
             .Message.ShouldBe("PlaceholderStart cannot contain the \uE002 character");
        }

        [Fact]
        public void Throws_On_PlaceholderEnd_With_Unsupported_Character()
        {
            // Arrange
            var builder = new FormattableStringParserSettingsBuilder();
            builder.WithPlaceholderEnd("\uE002");

            // Act & Assert
            Action a = () => builder.Build();
            a.ShouldThrow<ValidationException>()
             .Message.ShouldBe("PlaceholderEnd cannot contain the \uE002 character");
        }

        [Fact]
        public void Throws_On_PlaceholderStart_Same_As_PlaceholderEnd()
        {
            // Arrange
            var builder = new FormattableStringParserSettingsBuilder();
            builder.WithPlaceholderStart("x").WithPlaceholderEnd("x");

            // Act & Assert
            Action a = () => builder.Build();
            a.ShouldThrow<ValidationException>()
             .Message.ShouldBe("PlaceholderStart and PlaceholderEnd cannot have the same value");
        }
    }
}
