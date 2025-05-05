namespace CrossCutting.Common.Tests.Extensions;

public partial class StringExtensionsTests
{
    public class FindAllOccurences_Char
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var result = "my string".FindAllOccurences('g').ToArray();

            // Act & Assert
            result.Length.ShouldBe(1);
            result[0].ShouldBe("my string".IndexOf('g', default(StringComparison)));
        }
    }

    public class FindAllOccurences_String
    {
        [Fact]
        public void Throws_On_Empty_Find_Argument()
        {
            // Arrange
            Action a = () => _ = "my string".FindAllOccurences(stringToFind: string.Empty, default).ToArray();

            // Act & Assert
            a.ShouldThrow<ArgumentException>().ParamName.ShouldBe("stringToFind");
        }

        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var result = "my string".FindAllOccurences("g", default).ToArray();

            // Act & Assert
            result.Length.ShouldBe(1);
#pragma warning disable CA1866 // Use char overload
            result[0].ShouldBe("my string".IndexOf("g", default(StringComparison)));
#pragma warning restore CA1866 // Use char overload
        }
    }
}
