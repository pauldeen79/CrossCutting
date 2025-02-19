namespace CrossCutting.Common.Tests.Extensions;

public class FormatProviderExtensionsTests
{
    public class ToCultureInfo : FormatProviderExtensionsTests
    {
        [Fact]
        public void Returns_CultureInfo_When_Value_Could_Be_Cast()
        {
            // Arrange
            var sut = CultureInfo.GetCultureInfo("nl-NL");

            // Act
            var result = sut.ToCultureInfo();

            // Assert
            result.ShouldBeSameAs(sut);
        }

        [Fact]
        public void Returns_CurrentCulture_When_Value_Could_Not_Be_Cast()
        {
            // Arrange
            var sut = Substitute.For<IFormatProvider>();

            // Act
            var result = sut.ToCultureInfo();

            // Assert
            result.ShouldBeSameAs(CultureInfo.CurrentCulture);
        }
    }
}
